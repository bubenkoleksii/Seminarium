namespace SchoolService.Application.StudyPeriod.Commands.DeleteStudyPeriod;

public class DeleteStudyPeriodCommandHandler(ISchoolProfileManager schoolProfileManager, ICommandContext commandContext, IPublishEndpoint publishEndpoint)
    : IRequestHandler<DeleteStudyPeriodCommand, Option<Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    private readonly ICommandContext _commandContext = commandContext;

    public async Task<Option<Error>> Handle(DeleteStudyPeriodCommand request, CancellationToken cancellationToken)
    {
        var period = await _commandContext.StudyPeriods.FindAsync(request.Id, CancellationToken.None);
        if (period is null)
            return new NotFoundByIdError(request.Id);

        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null || profile.Type != SchoolProfileType.SchoolAdmin || profile.SchoolId != period.SchoolId)
            return new InvalidError("school_profile");

        try
        {
            _commandContext.StudyPeriods.Remove(period);
            await _commandContext.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(new DeleteCoursesRequest(request.Id), cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the study period with ID {@SchoolId}.", request.Id);

            return new InvalidDatabaseOperationError("study_period");
        }

        return Option<Error>.None;
    }
}
