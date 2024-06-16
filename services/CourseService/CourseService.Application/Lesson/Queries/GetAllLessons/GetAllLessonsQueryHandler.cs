namespace CourseService.Application.Lesson.Queries.GetAllLessons;

public class GetAllLessonsQueryHandler(
    IMapper mapper,
    IQueryContext queryContext
    ) : IRequestHandler<GetAllLessonsQuery, Either<GetAllLessonsModelResponse, Error>>
{
    private const int DefaultTake = 10;

    private readonly IMapper _mapper = mapper;

    private readonly IQueryContext _queryContext = queryContext;

    public async Task<Either<GetAllLessonsModelResponse, Error>> Handle(GetAllLessonsQuery request, CancellationToken cancellationToken)
    {
        var course = await _queryContext.Courses.FindAsync(request.CourseId, CancellationToken.None);
        if (course is null)
            return new InvalidError("course");

        var dbQuery = _queryContext.Lessons
            .Where(l => l.CourseId == course.Id)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Topic))
            dbQuery = dbQuery.Where(l => l.Topic.ToLower().Contains(request.Topic.ToLower()));

        dbQuery = dbQuery.OrderByDescending(l => l.Number);

        var entities = await dbQuery
            .ToListAsync(cancellationToken: cancellationToken);

        var modelResponses = _mapper.Map<IEnumerable<LessonModelResponse>>(entities);
        foreach (var modelResponse in modelResponses)
        {
            var practicalItemsCount = await _queryContext.PracticalLessonItems
                .Where(item => item.LessonId == modelResponse.Id)
                .CountAsync(cancellationToken: cancellationToken);

            var theoryItemsCount = await _queryContext.PracticalLessonItems
                .Where(item => item.LessonId == modelResponse.Id)
                .CountAsync(cancellationToken: cancellationToken);

            modelResponse.PracticalItemsCount = (uint)practicalItemsCount;
            modelResponse.TheoryItemsCount = (uint)theoryItemsCount;
        }

        var response = new GetAllLessonsModelResponse(
            Entries: modelResponses,
            CourseId: request.CourseId,
            Total: (ulong)dbQuery.Count(),
            Skip: 0,
            Take: DefaultTake
        );

        return response;
    }
}
