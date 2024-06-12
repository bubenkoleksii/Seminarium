namespace SchoolService.Application.Group.Consumers;

public class GetGroupsConsumer(IQueryContext queryContext, IMapper mapper) : IConsumer<GetGroupsRequest>
{
    private readonly IQueryContext _queryContext = queryContext;

    private readonly IMapper _mapper = mapper;

    public async Task Consume(ConsumeContext<GetGroupsRequest> context)
    {
        var response = new GetGroupsResponse();

        var dbQuery = _queryContext
            .Groups
            .AsQueryable();

        if (context.Message.SchoolId.HasValue)
            dbQuery = dbQuery.Where(group => group.SchoolId == context.Message.SchoolId.Value);

        if (context.Message.Ids != null && context.Message.Ids.Length > 0)
            dbQuery = dbQuery.Where(group => context.Message.Ids.Contains(group.Id));

        var entities = await dbQuery.ToListAsync();

        response.Groups = _mapper.Map<IEnumerable<GroupContract>>(entities);
        await context.RespondAsync(response);
    }
}
