namespace SchoolService.Application.JoiningRequest.Commands.Queries.GetAllJoiningRequests;

public class GetAllJoiningRequestsHandler : IRequestHandler<GetAllJoiningRequestsQuery, GetAllJoiningRequestsModelResponse>
{
    private const uint DefaultTake = 10;

    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    public GetAllJoiningRequestsHandler(IQueryContext queryContext, IMapper mapper)
    {
        _queryContext = queryContext;
        _mapper = mapper;
    }

    public async Task<GetAllJoiningRequestsModelResponse> Handle(GetAllJoiningRequestsQuery request, CancellationToken cancellationToken)
    {
        var dbQuery = _queryContext.JoiningRequests.AsQueryable();

        if (!string.IsNullOrEmpty(request.SchoolName))
            dbQuery = dbQuery.Where(r => r.Name.ToLower().Contains(request.SchoolName.ToLower()));

        if (request.SortByDateAsc.HasValue)
        {
            dbQuery = request.SortByDateAsc.Value
                ? dbQuery.OrderBy(r => r.CreatedAt)
                : dbQuery.OrderByDescending(r => r.CreatedAt);
        }

        if (request.Region.HasValue)
            dbQuery = dbQuery.Where(r => r.Region == request.Region);

        var take = request.Take ?? DefaultTake;
        var entities = await dbQuery
            .Skip((int)request.Skip)
            .Take((int)take)
            .ToListAsync(cancellationToken: cancellationToken);

        var joiningRequestsResponse = _mapper.Map<IEnumerable<JoiningRequestModelResponse>>(entities);

        var response = new GetAllJoiningRequestsModelResponse(
            Entries: joiningRequestsResponse,
            Total: (ulong)dbQuery.Count(),
            Skip: request.Skip,
            Take: take
        );

        return response;
    }
}
