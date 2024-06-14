namespace CourseService.Application.Course.Commands.CreateCourse;

public class CreateCourseCommandHandler(
    ICommandContext commandContext,
    IMapper mapper,
    ISchoolProfileAccessor schoolProfileAccessor,
    IRequestClient<GetStudyPeriodsRequest> studyPeriodsClient
    ) : IRequestHandler<CreateCourseCommand, Either<CourseModelResponse, Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IRequestClient<GetStudyPeriodsRequest> _studyPeriodsClient = studyPeriodsClient;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<CourseModelResponse, Error>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
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

        var teacher = new CourseTeacher
        {
            Id = activeProfile.Id,
        };

        var entity = _mapper.Map<Domain.Entities.Course>(request);
        entity.Teachers = [teacher];

        await _commandContext.Courses.AddAsync(entity, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the course with values {@Request}.", request);
            return new InvalidDatabaseOperationError("course");
        }

        var modelResponse = _mapper.Map<CourseModelResponse>(entity);
        return modelResponse;
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
