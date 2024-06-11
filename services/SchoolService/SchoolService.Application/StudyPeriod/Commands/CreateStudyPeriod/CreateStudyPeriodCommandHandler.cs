namespace SchoolService.Application.StudyPeriod.Commands.CreateStudyPeriod;

public class CreateStudyPeriodCommandHandler(ISchoolProfileManager schoolProfileManager, ICommandContext commandContext, IMapper mapper)
    : IRequestHandler<CreateStudyPeriodCommand, Either<StudyPeriodModelResponse, Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly ICommandContext _commandContext = commandContext;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<StudyPeriodModelResponse, Error>> Handle(CreateStudyPeriodCommand request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null || profile.Type != SchoolProfileType.SchoolAdmin)
            return new InvalidError("school_profile");

        var school = await _commandContext.Schools.FindAsync(profile.SchoolId, CancellationToken.None);
        if (school == null)
            return new InvalidError("school_id");

        if (profile.Type != SchoolProfileType.SchoolAdmin || profile.SchoolId != school.Id)
            return new InvalidError("school_profile");

        try
        {
            var existedEntity = await _commandContext.StudyPeriods
                .AsNoTracking()
                .Where(period => period.StartDate == request.StartDate && period.EndDate == request.EndDate)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (existedEntity is not null)
                return new AlreadyExistsError("study_period");
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the study period with values {@Request}.", request);
        }

        var entity = _mapper.Map<Domain.Entities.StudyPeriod>(request);
        entity.School = school;

        await _commandContext.StudyPeriods.AddAsync(entity, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the study period with values {@Request}.", request);
            return new InvalidDatabaseOperationError("study_period");
        }

        var studyPeriodResponse = _mapper.Map<StudyPeriodModelResponse>(entity);

        if (request.IncrementGroups)
        {
            var groups = await _commandContext.Groups
                .Where(group => group.SchoolId == school.Id)
                .ToListAsync(CancellationToken.None);

            if (groups is not null)
            {
                foreach (var group in groups)
                    group.StudyPeriodNumber++;

                _commandContext.Groups.UpdateRange(groups);

                try
                {
                    await _commandContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "An error occurred while incrementing the study period number of group with values {@Request}.", request);
                    return new InvalidDatabaseOperationError("group");
                }

                studyPeriodResponse.IncrementGroups = true;
            }
        }

        return studyPeriodResponse;
    }
}
