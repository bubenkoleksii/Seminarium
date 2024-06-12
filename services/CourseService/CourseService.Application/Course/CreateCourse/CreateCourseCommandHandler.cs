namespace CourseService.Application.Course.CreateCourse;

public class CreateCourseCommandHandler(IRequestClient<GetActiveSchoolProfileRequest> schoolProfileClient)
    : IRequestHandler<CreateCourseCommand, string>
{
    private readonly IRequestClient<GetActiveSchoolProfileRequest> _schoolProfileClient = schoolProfileClient;

    public async Task<string> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var getActiveSchoolProfileRequest = new GetActiveSchoolProfileRequest(
            request.UserId,
            [Constants.ClassTeacher, Constants.SchoolAdmin, Constants.Teacher]
        );

        var response = await _schoolProfileClient.GetResponse<GetActiveSchoolProfileResponse>(getActiveSchoolProfileRequest, cancellationToken);
        if (response == null)
        {
            return "1";
        }
        else return "34";
    }
}
