namespace CourseService.Application.Course.CreateCourse;

public class CreateCourseCommandHandler(IRequestClient<GetActiveSchoolProfileRequest> schoolProfileClient)
    : IRequestHandler<CreateCourseCommand, int>
{
    private readonly IRequestClient<GetActiveSchoolProfileRequest> _schoolProfileClient = schoolProfileClient;

    public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var getActiveSchoolProfileRequest = new GetActiveSchoolProfileRequest(
            request.UserId,
            [Constants.ClassTeacher, Constants.SchoolAdmin, Constants.Teacher]
        );

        var response = await _schoolProfileClient.GetResponse<Either<GetSchoolProfileResponse, Error>>(getActiveSchoolProfileRequest, cancellationToken);
    }
}
