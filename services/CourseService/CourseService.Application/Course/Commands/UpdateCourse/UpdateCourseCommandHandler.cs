namespace CourseService.Application.Course.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(
    ICommandContext commandContext,
    IMapper mapper,
    ISchoolProfileAccessor schoolProfileAccessor,
    IRequestClient<GetStudyPeriodsRequest> studyPeriodsClient
    ) : IRequestHandler<UpdateCourseCommand, Either<CourseModelResponse, Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IRequestClient<GetStudyPeriodsRequest> _studyPeriodsClient = studyPeriodsClient;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<CourseModelResponse, Error>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
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

        var course = await _commandContext.Courses.FindAsync(request.Id, CancellationToken.None);
        if (course == null)
            return new NotFoundByIdError(request.Id, "course");

        if (activeProfile.Type != Constants.Teacher && activeProfile.Type != Constants.SchoolAdmin)
            return new InvalidError("school_profile");

        if (activeProfile.Type == Constants.Teacher)
        {
            var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(course.Id, activeProfile.Id);
            if (teacherValidatingResult.IsSome)
                return (Error)teacherValidatingResult;
        }

        _mapper.Map(request, course);

        _commandContext.Courses.Update(course);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while updating the course with values {@Request}.", request);

            return new InvalidDatabaseOperationError("course");
        }

        var courseModelResponse = _mapper.Map<CourseModelResponse>(course);
        return courseModelResponse;
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
