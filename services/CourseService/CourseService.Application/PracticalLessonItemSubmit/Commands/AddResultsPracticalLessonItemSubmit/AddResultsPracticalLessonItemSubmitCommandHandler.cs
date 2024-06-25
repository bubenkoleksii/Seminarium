using CourseService.Application.Common.Email;

namespace CourseService.Application.PracticalLessonItemSubmit.Commands.AddResultsPracticalLessonItemSubmit;

public class AddResultsPracticalLessonItemSubmitCommandHandler(
    ICommandContext commandContext,
    ISchoolProfileAccessor schoolProfileAccessor,
    IMailService mailService,
    IConfiguration configuration,
    IRequestClient<GetSchoolProfilesRequest> getSchoolProfilesClient
    ) : IRequestHandler<AddResultsPracticalLessonItemSubmitCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IMailService _mailService = mailService;

    private readonly IConfiguration _configuration = configuration;

    private readonly IRequestClient<GetSchoolProfilesRequest> _getSchoolProfilesClient = getSchoolProfilesClient;

    public async Task<Option<Error>> Handle(AddResultsPracticalLessonItemSubmitCommand request, CancellationToken cancellationToken)
    {
        var getActiveProfileRequest = new GetActiveSchoolProfileRequest(
            UserId: request.UserId,
            AllowedProfileTypes: [Constants.Teacher]
        );

        var retrievingActiveProfileResult =
            await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);

        if (retrievingActiveProfileResult.IsRight)
            return (Error)retrievingActiveProfileResult;

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null || activeProfile.SchoolId == null)
            return new InvalidError("school_profile");

        var practicalLessonItemSubmit = await _commandContext.PracticalLessonItemSubmits
            .Include(s => s.PracticalLessonItem)
            .FirstOrDefaultAsync(s => s.Id == request.Id);

        if (practicalLessonItemSubmit == null)
            return new NotFoundByIdError(request.Id, "school-profile-submit");

        var teacher = await _commandContext.CourseTeachers
            .Include(t => t.Courses)
            .FirstOrDefaultAsync(t => t.Id == activeProfile.Id, CancellationToken.None);
        if (teacher == null)
            return new InvalidError("teacher");

        var lesson = await _commandContext.Lessons.FindAsync(practicalLessonItemSubmit.PracticalLessonItem.LessonId);
        if (lesson == null)
            return new InvalidError("lesson");

        if (teacher.Courses == null || !teacher.Courses.Any(c => c.Id == lesson.CourseId))
            return new InvalidError("teacher");

        practicalLessonItemSubmit.TeacherComment = request.Text;
        practicalLessonItemSubmit.Status = request.IsAccept
            ? PracticalLessonItemSubmitStatus.Accepted
            : PracticalLessonItemSubmitStatus.Rejected;

        _commandContext.PracticalLessonItemSubmits.Update(practicalLessonItemSubmit);

        if (request.IsAccept)
            practicalLessonItemSubmit.Mark = request.Mark;

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while adding result the practical lesson item submit with values {@Request}.", request);

            return new InvalidDatabaseOperationError("practical_lesson_item_submit");
        }

        var student = await GetStudent(practicalLessonItemSubmit.StudentId, cancellationToken);
        if (student == null || student.Email == null)
            return Option<Error>.None;

        var clientUrl = _configuration["ClientUrl"]!;
        await _mailService.SendAsync(
                          student.Email,
                          EmailTemplates.AddResultsToPracticalItemSubmit.Subject,
                          EmailTemplates.AddResultsToPracticalItemSubmit.GetTemplate(
                              activeProfile.Name,
                              practicalLessonItemSubmit.PracticalLessonItem.Title,
                              clientUrl,
                              request.Text));

        return Option<Error>.None;
    }

    private async Task<SchoolProfileContract?> GetStudent(Guid studentId, CancellationToken cancellationToken)
    {
        var getStudentRequest = new GetSchoolProfilesRequest(Ids: [studentId], null, null, null, null);

        try
        {
            var response = await _getSchoolProfilesClient.GetResponse<GetSchoolProfilesResponse>(getStudentRequest, cancellationToken);

            if (response.Message.Profiles == null || !response.Message.Profiles.Any())
                return null;

            return response.Message.Profiles.First();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while sending get profiles request", getStudentRequest);
            return null;
        }
    }
}
