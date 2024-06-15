namespace CourseService.Application.Course.Queries.GetOneCourse;

public class GetOneCourseQueryHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    IRequestClient<GetSchoolProfilesRequest> getSchoolProfilesClient,
    IRequestClient<GetGroupsRequest> getGroupsClient,
    IMapper mapper,
    IQueryContext queryContext
    ) : IRequestHandler<GetOneCourseQuery, Either<CourseModelResponse, Error>>
{
    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IRequestClient<GetSchoolProfilesRequest> _getSchoolProfilesClient = getSchoolProfilesClient;

    private readonly IRequestClient<GetGroupsRequest> _getGroupsClient = getGroupsClient;

    private readonly IMapper _mapper = mapper;

    private readonly IQueryContext _queryContext = queryContext;

    public async Task<Either<CourseModelResponse, Error>> Handle(GetOneCourseQuery request, CancellationToken cancellationToken)
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

        var course = await _queryContext.Courses
                   .IgnoreQueryFilters()
                   .Include(c => c.Groups)
                   .Include(c => c.Teachers)
                   .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
            return new NotFoundByIdError(request.Id, "course");

        RemoveCycling(course);

        var courseResponse = _mapper.Map<CourseModelResponse>(course);
        courseResponse.Teachers = await GetTeachers(courseResponse.Teachers, cancellationToken);
        courseResponse.Groups = await GetGroups(courseResponse.Groups, cancellationToken);

        return courseResponse;
    }

    private static void RemoveCycling(Domain.Entities.Course course)
    {
        if (course == null)
            return;

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

    private async Task<IEnumerable<CourseTeacherModelResponse>> GetTeachers(IEnumerable<CourseTeacherModelResponse>? courseTeachers, CancellationToken cancellationToken)
    {
        if (courseTeachers is null || !courseTeachers.Any())
            return [];

        var teachersIds = courseTeachers.Select(t => t.Id).ToArray();
        if (teachersIds.Length == 0)
            return [];

        var getTeachersRequest = new GetSchoolProfilesRequest(Ids: teachersIds, null, null, null, null);

        try
        {
            var response = await _getSchoolProfilesClient.GetResponse<GetSchoolProfilesResponse>(getTeachersRequest, cancellationToken);

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

    private async Task<IEnumerable<CourseGroupModelResponse>> GetGroups(IEnumerable<CourseGroupModelResponse>? courseGroups, CancellationToken cancellationToken)
    {
        if (courseGroups == null || !courseGroups.Any())
            return [];

        var groupsIds = courseGroups.Select(g => g.Id).ToArray();

        if (groupsIds.Length == 0)
            return [];

        var getGroupsRequest = new GetGroupsRequest(groupsIds, null);

        try
        {
            var response = await _getGroupsClient.GetResponse<GetGroupsResponse>(getGroupsRequest, cancellationToken);

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
}
