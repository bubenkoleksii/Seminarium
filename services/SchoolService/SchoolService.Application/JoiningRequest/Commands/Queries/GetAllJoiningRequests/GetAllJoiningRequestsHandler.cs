namespace SchoolService.Application.JoiningRequest.Commands.Queries.GetAllJoiningRequests;

public class GetAllJoiningRequestsHandler : IRequestHandler<GetAllJoiningRequestsQuery, IEnumerable<JoiningRequestModelResponse>>
{
    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    public GetAllJoiningRequestsHandler(IQueryContext queryContext, IMapper mapper)
    {
        _queryContext = queryContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<JoiningRequestModelResponse>> Handle(GetAllJoiningRequestsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _queryContext.JoiningRequests.ToListAsync(cancellationToken: cancellationToken);

        var joiningRequestsResponse = _mapper.Map<IEnumerable<JoiningRequestModelResponse>>(entities);
        return joiningRequestsResponse;
    }
}
