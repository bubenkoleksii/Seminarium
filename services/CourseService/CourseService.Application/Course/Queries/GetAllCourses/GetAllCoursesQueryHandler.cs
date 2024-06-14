namespace CourseService.Application.Course.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    IRequestClient<GetSchoolProfilesRequest> getSchoolProfilesClient,
    IRequestClient<GetGroupsRequest> getGroupsClient,
    IMapper mapper,
    IQueryContext queryContext,
    IRequestClient<GetStudyPeriodsRequest> studyPeriodsClient
    ) : IRequestHandler<GetAllCoursesQuery, Either<GetAllCoursesModelResponse, Error>>
{
    private const int DefaultTake = 4;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IRequestClient<GetSchoolProfilesRequest> _getSchoolProfilesClient = getSchoolProfilesClient;

    private readonly IRequestClient<GetGroupsRequest> _getGroupsClient = getGroupsClient;

    private readonly IMapper _mapper = mapper;

    private readonly IQueryContext _queryContext = queryContext;

    private readonly IRequestClient<GetStudyPeriodsRequest> _studyPeriodsClient = studyPeriodsClient;

    public async Task<Either<GetAllCoursesModelResponse, Error>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var getActiveProfileRequest = new GetActiveSchoolProfileRequest(
            UserId: request.UserId,
            AllowedProfileTypes: [Constants.Teacher, Constants.ClassTeacher, Constants.SchoolAdmin]
        );

        var retrievingActiveProfileResult = await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);
        if (retrievingActiveProfileResult.IsRight)
            return (Error)retrievingActiveProfileResult;

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null || activeProfile.SchoolId == null)
            return new InvalidError("school_profile");

        var studyPeriodValidatingResult = await ValidateStudyPeriod(request.StudyPeriodId, activeProfile.SchoolId.Value, cancellationToken);
        if (studyPeriodValidatingResult.IsSome)
            return (Error)studyPeriodValidatingResult;

        var dbQuery = _queryContext.Courses
            .Include(c => c.Groups)
            .Include(c => c.Teachers)
            .Where(course => course.StudyPeriodId == request.StudyPeriodId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Name))
            dbQuery = dbQuery.Where(c => c.Name.ToLower().Contains(request.Name.ToLower()));

        var courses = await dbQuery.ToListAsync();

        if (request.GroupId.HasValue)
        {
            var group = await _queryContext.CourseGroups.FindAsync(request.GroupId.Value, CancellationToken.None);

            if (group == null)
                return new NotFoundByIdError(request.GroupId.Value, "group");

            courses = courses
                .Where(c => c.Groups != null && c.Groups.Any(g => g.Id == group.Id))
                .ToList();
        }

        if (request.TeacherId.HasValue)
        {
            var teacher = await _queryContext.CourseTeachers.FindAsync(request.TeacherId.Value, CancellationToken.None);

            if (teacher == null)
                return new NotFoundByIdError(request.TeacherId.Value, "teacher");

            courses = courses
                 .Where(c => c.Teachers != null && c.Teachers.Any(g => g.Id == teacher.Id))
                .ToList();
        }

        var take = request.Take ?? DefaultTake;

        courses = courses
            .Skip((int)request.Skip)
            .Take((int)take)
            .ToList();

        RemoveCycling(courses);

        var coursesResponses = _mapper.Map<IEnumerable<CourseModelResponse>>(courses);
        foreach (var courseResponse in coursesResponses)
        {
            courseResponse.Teachers = await GetTeachers(courseResponse.Teachers, cancellationToken);
            courseResponse.Groups = await GetGroups(courseResponse.Groups, cancellationToken);
        }

        var response = new GetAllCoursesModelResponse(
            Entries: coursesResponses,
            StudyPeriodId: request.StudyPeriodId,
            Total: (ulong)dbQuery.Count(),
            Skip: request.Skip,
            Take: take
        );

        return response;
    }

    private static void RemoveCycling(IEnumerable<Domain.Entities.Course> courses)
    {
        if (courses == null || !courses.Any())
            return;

        foreach (var course in courses)
        {
            if (course.Teachers != null)
            {
                foreach (var teacher in course.Teachers)
                {
                    teacher.Courses = null;
                }
            }

            if (course.Groups != null)
            {
                foreach (var group in course.Groups)
                {
                    group.Courses = null;
                }
            }
        }
    }

    private async Task<IEnumerable<CourseTeacherModelResponse>> GetTeachers
        (IEnumerable<CourseTeacherModelResponse>? courseTeachers, CancellationToken cancellationToken)
    {
        if (courseTeachers is null || !courseTeachers.Any())
            return [];

        var teachersIds = courseTeachers.Select(t => t.Id).ToArray();
        if (teachersIds.Length == 0)
            return [];

        var getTeachersRequest = new GetSchoolProfilesRequest(Ids: teachersIds, null, null, null, null);

        try
        {
            var response =
                await _getSchoolProfilesClient.GetResponse<GetSchoolProfilesResponse>(getTeachersRequest, cancellationToken);

            if (response.Message.Profiles == null)
                return [];

            var teachers = new List<CourseTeacherModelResponse>();
            foreach (var profile in response.Message.Profiles)
            {
                var teacher = new CourseTeacherModelResponse(
                    Id: profile.Id,
                    Name: profile.Name,
                    IsCreator: courseTeachers.First(t => t.Id == profile.Id).IsCreator
                );

                teachers.Add(teacher);
            }

            return teachers;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while sending get profiles with values @Request", getTeachersRequest);
            return [];
        }
    }

    private async Task<IEnumerable<CourseGroupModelResponse>> GetGroups
    (IEnumerable<CourseGroupModelResponse>? courseGroups, CancellationToken cancellationToken)
    {
        if (courseGroups == null || !courseGroups.Any())
            return [];

        var groupsIds = courseGroups.Select(g => g.Id).ToArray();

        if (groupsIds.Length == 0)
            return [];

        var getGroupsRequest = new GetGroupsRequest(groupsIds, null);

        try
        {
            var response =
                await _getGroupsClient.GetResponse<GetGroupsResponse>(getGroupsRequest, cancellationToken);

            if (response.Message.Groups == null)
                return [];

            var groups = new List<CourseGroupModelResponse>();
            foreach (var groupResult in response.Message.Groups)
            {
                var group = new CourseGroupModelResponse(
                    Id: groupResult.Id,
                    Name: groupResult.Name
                );

                groups.Add(group);
            }

            return groups;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while sending get profiles with values @Request", getGroupsRequest);
            return [];
        }
    }

    private async Task<Option<Error>> ValidateStudyPeriod(Guid studyPeriodId, Guid schoolId, CancellationToken cancellationToken)
    {
        var getStudyPeriodsRequest = new GetStudyPeriodsRequest(Ids: [studyPeriodId], SchoolId: null);

        try
        {
            var response = await _studyPeriodsClient
                .GetResponse<GetStudyPeriodsResponse>(getStudyPeriodsRequest, cancellationToken);

            var isStudyPeriodInvalid = response.Message.HasError ||
                response.Message.StudyPeriods == null || !response.Message.StudyPeriods.Any() ||
                response.Message.StudyPeriods.FirstOrDefault()?.SchoolId != schoolId;
            if (isStudyPeriodInvalid)
                return new InvalidError("study_period");

            return Option<Error>.None;
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while sending get study period message @Id", studyPeriodId);
            return new InternalServicesError("school");
        }
    }
}
