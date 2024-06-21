namespace SchoolService.Application.StudyPeriod.Consumers;

public class GetStudyPeriodsConsumer(IQueryContext queryContext, IMapper mapper) : IConsumer<GetStudyPeriodsRequest>
{
    private readonly IQueryContext _queryContext = queryContext;

    private readonly IMapper _mapper = mapper;

    public async Task Consume(ConsumeContext<GetStudyPeriodsRequest> context)
    {
        var response = new GetStudyPeriodsResponse();

        var dbQuery = _queryContext
            .StudyPeriods
            .AsQueryable();

        if (context.Message.SchoolId.HasValue)
            dbQuery = dbQuery.Where(period => period.SchoolId == context.Message.SchoolId.Value);

        if (context.Message.Ids != null && context.Message.Ids.Length > 0)
            dbQuery = dbQuery.Where(period => context.Message.Ids.Contains(period.Id));

        var entities = await dbQuery.ToListAsync();

        response.StudyPeriods = _mapper.Map<IEnumerable<StudyPeriodContact>>(entities);
        await context.RespondAsync(response);
    }
}
