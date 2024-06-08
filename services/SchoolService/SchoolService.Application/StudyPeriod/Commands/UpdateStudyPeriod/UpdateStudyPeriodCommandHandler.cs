namespace SchoolService.Application.StudyPeriod.Commands.UpdateStudyPeriod;

public class UpdateStudyPeriodCommandHandler(
    ISchoolProfileManager schoolProfileManager,
    ICommandContext commandContext,
    IMapper mapper) : IRequestHandler<UpdateStudyPeriodCommand, Either<StudyPeriodModelResponse, Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly ICommandContext _commandContext = commandContext;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<StudyPeriodModelResponse, Error>> Handle(UpdateStudyPeriodCommand request, CancellationToken cancellationToken)
    {
        var period = await _commandContext.StudyPeriods.FindAsync(request.Id, CancellationToken.None);
        if (period is null)
            return new NotFoundByIdError(request.Id);

        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null || profile.Type != SchoolProfileType.SchoolAdmin || profile.SchoolId != period.SchoolId)
            return new InvalidError("school_profile");

        _mapper.Map(request, period);

        _commandContext.StudyPeriods.Update(period);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while updating the study period with values {@Request}.", request);

            return new InvalidDatabaseOperationError("study_period");
        }

        var periodResponse = _mapper.Map<StudyPeriodModelResponse>(period);
        return periodResponse;
    }
}
