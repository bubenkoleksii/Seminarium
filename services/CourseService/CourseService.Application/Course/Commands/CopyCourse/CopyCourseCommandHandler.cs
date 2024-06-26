namespace CourseService.Application.Course.Commands.CopyCourse;

public class CopyCourseCommandHandler(
    ICommandContext commandContext,
    IQueryContext queryContext,
    IMapper mapper,
    ISchoolProfileAccessor schoolProfileAccessor,
    IRequestClient<GetStudyPeriodsRequest> studyPeriodsClient
    ) : IRequestHandler<CopyCourseCommand, Either<CourseModelResponse, Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly IQueryContext _queryContext = queryContext;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IRequestClient<GetStudyPeriodsRequest> _studyPeriodsClient = studyPeriodsClient;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<CourseModelResponse, Error>> Handle(CopyCourseCommand request, CancellationToken cancellationToken)
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

        var existedCourse = await _queryContext.Courses
            .AsNoTracking()
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == request.Id, CancellationToken.None);
        if (existedCourse == null)
            return new NotFoundByIdError(request.Id, "course");

        var newCourse = new Domain.Entities.Course
        {
            StudyPeriodId = request.StudyPeriodId,
            Name = request.Name ?? existedCourse.Name,
            Description = request.Description ?? existedCourse.Description,
        };

        if (existedCourse.Lessons != null && existedCourse.Lessons.Count != 0)
        {
            var lessons = new List<Domain.Entities.Lesson>();
            foreach (var existedLesson in existedCourse.Lessons)
            {
                var lesson = new Domain.Entities.Lesson
                {
                    Course = newCourse,
                    Topic = existedLesson.Topic,
                    Number = existedLesson.Number,
                    Homework = existedLesson.Homework,
                };

                lessons.Add(lesson);
            }

            newCourse.Lessons = lessons;
        }

        newCourse.Id = Guid.Empty;

        if (activeProfile.Type == Constants.Teacher)
        {
            var courseTeacher = await _commandContext.CourseTeachers.FindAsync(activeProfile.Id, cancellationToken);

            if (courseTeacher != null)
                newCourse.Teachers = [courseTeacher];
        }

        if (activeProfile.Type == Constants.ClassTeacher)
        {
            var courseGroup = await _commandContext.CourseGroups.FindAsync(activeProfile.ClassTeacherGroupId, cancellationToken);

            if (courseGroup != null)
                newCourse.Groups = [courseGroup];
        }

        await _commandContext.Courses.AddAsync(newCourse, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while copying the course with values {@Request}.", request);
            return new InvalidDatabaseOperationError("course");
        }

        var modelResponse = _mapper.Map<CourseModelResponse>(newCourse);
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
