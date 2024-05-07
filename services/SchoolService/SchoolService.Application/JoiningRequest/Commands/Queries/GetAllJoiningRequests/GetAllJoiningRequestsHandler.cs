namespace SchoolService.Application.JoiningRequest.Commands.Queries.GetAllJoiningRequests;

public class GetAllJoiningRequestsHandler : IRequestHandler<GetAllJoiningRequestsQuery, GetAllJoiningRequestsModelResponse>
{
    private const int DefaultTake = 5;

    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    public GetAllJoiningRequestsHandler(IQueryContext queryContext, IMapper mapper)
    {
        _queryContext = queryContext;
        _mapper = mapper;
    }

    public async Task<GetAllJoiningRequestsModelResponse> Handle(GetAllJoiningRequestsQuery request, CancellationToken cancellationToken)
    {
        var take = request.Take ?? DefaultTake;

        var entities = await _queryContext.JoiningRequests.ToListAsync(cancellationToken: cancellationToken);

        var joiningRequestsResponse = _mapper.Map<IEnumerable<JoiningRequestModelResponse>>(entities);

        var response = new GetAllJoiningRequestsModelResponse(
            Entries: joiningRequestsResponse,
            Total: 1,
            Skip: 1,
            Take: take
        );

        return response;
    }
}
