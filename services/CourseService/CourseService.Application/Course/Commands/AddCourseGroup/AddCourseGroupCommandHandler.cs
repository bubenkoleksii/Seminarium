namespace CourseService.Application.Course.Commands.AddCourseGroup;

public class AddCourseGroupCommandHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    IRequestClient<GetGroupsRequest> getGroupsClient,
    ICommandContext commandContext
    ) : IRequestHandler<AddCourseGroupCommand, Either<CourseGroupModelResponse, Error>>
{
    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IRequestClient<GetGroupsRequest> _getGroupsClient = getGroupsClient;

    private readonly ICommandContext _commandContext = commandContext;

    public async Task<Either<CourseGroupModelResponse, Error>> Handle(AddCourseGroupCommand request, CancellationToken cancellationToken)
    {
        var getActiveProfileRequest = new GetActiveSchoolProfileRequest(
            UserId: request.UserId,
            AllowedProfileTypes: [Constants.Teacher, Constants.ClassTeacher, Constants.SchoolAdmin]
        );

        var retrievingActiveProfileResult = await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);
        if (retrievingActiveProfileResult.IsRight)
            return (Error)retrievingActiveProfileResult;

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null || activeProfile.SchoolId == null ||
            (activeProfile.Type != Constants.SchoolAdmin && activeProfile.Type != Constants.Teacher))
            return new InvalidError("school_profile");

        var course = await _commandContext.Courses.FindAsync(request.CourseId, CancellationToken.None);
        if (course == null)
            return new InvalidError("course");

        var getGroupsRequest = new GetGroupsRequest(Ids: null, SchoolId: request.SchoolId);

        var groupsResponse =
                await _getGroupsClient.GetResponse<GetGroupsResponse>(getGroupsRequest, cancellationToken);
        if (groupsResponse.Message.HasError)
            return (Error)groupsResponse;

        if (groupsResponse.Message.Groups == null || !groupsResponse.Message.Groups.Any())
            return new NotFoundError("group");

        var filteredGroup = groupsResponse.Message.Groups
            .FirstOrDefault(group => string.Equals(group.Name, request.Name, StringComparison.OrdinalIgnoreCase));

        if (filteredGroup == null)
            return new NotFoundError("group");

        course.Groups ??= [];
        var existedGroup = await _commandContext.CourseGroups.FindAsync(filteredGroup.Id);
        if (existedGroup != null)
        {
            course.Groups.Add(existedGroup);
        }
        else
        {
            var newCourseGroup = new CourseGroup
            {
                Id = filteredGroup.Id
            };

            try
            {
                await _commandContext.CourseGroups.AddAsync(newCourseGroup, cancellationToken);
                await _commandContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "An error occurred while creating the course group with values {@Request}.", request);
                return new InvalidDatabaseOperationError("course");
            }

            course.Groups.Add(newCourseGroup);
        }

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the course group with values {@Request}.", request);
            return new InvalidDatabaseOperationError("course");
        }

        return new CourseGroupModelResponse(filteredGroup.Id, filteredGroup.Name);
    }
}
