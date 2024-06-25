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
            AllowedProfileTypes: [Constants.Teacher, Constants.ClassTeacher,
                Constants.SchoolAdmin, Constants.Student, Constants.Teacher]
        );

        var retrievingActiveProfileResult = await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);
        if (retrievingActiveProfileResult.IsRight)
            return (Error)retrievingActiveProfileResult;

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null || activeProfile.SchoolId == null)
            return new InvalidError("school_profile");

        var dbQuery = _queryContext.Courses
            .Include(c => c.Groups)
            .Include(c => c.Teachers)
            .AsQueryable();

        if (request.StudyPeriodId.HasValue)
        {
            var studyPeriodValidatingResult = await ValidateStudyPeriod(request.StudyPeriodId.Value, activeProfile.SchoolId.Value, cancellationToken);
            if (studyPeriodValidatingResult.IsSome)
                return (Error)studyPeriodValidatingResult;

            dbQuery = dbQuery.Where(course => course.StudyPeriodId == request.StudyPeriodId.Value);
        }
        else
            return new NotFoundError("study_period");

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
            var groupsIds = courses.Find(g => g.Id == courseResponse.Id)!.Groups?.Select(g => g.Id).ToArray();
            courseResponse.Groups = await GetGroups(groupsIds, cancellationToken);

            var teachersIds = courses.Find(g => g.Id == courseResponse.Id)!.Teachers?.Select(t => t.Id).ToArray();
            courseResponse.Teachers = await GetTeachers(teachersIds, cancellationToken);
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
        (Guid[]? teachersIds, CancellationToken cancellationToken)
    {
        if (teachersIds == null || teachersIds.Length == 0)
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
                    IsCreator: false
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
        (Guid[]? groupsIds, CancellationToken cancellationToken)
    {
        if (groupsIds == null || groupsIds.Length == 0)
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
