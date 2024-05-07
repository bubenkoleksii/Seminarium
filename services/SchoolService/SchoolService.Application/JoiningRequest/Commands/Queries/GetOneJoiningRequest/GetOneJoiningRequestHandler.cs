namespace SchoolService.Application.JoiningRequest.Commands.Queries.GetOneJoiningRequest;

public class GetOneJoiningRequestHandler : IRequestHandler<GetOneJoiningRequestQuery, Either<JoiningRequestModelResponse, Error>>
{
    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    public GetOneJoiningRequestHandler(IQueryContext queryContext, IMapper mapper)
    {
        _queryContext = queryContext;
        _mapper = mapper;
    }

    public async Task<Either<JoiningRequestModelResponse, Error>> Handle(GetOneJoiningRequestQuery request, CancellationToken cancellationToken)
    {
        var entity = await _queryContext.JoiningRequests
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: CancellationToken.None);

        if (entity is null)
            return new NotFoundByIdError(request.Id, "joining request");

        var joiningRequestResponse = _mapper.Map<JoiningRequestModelResponse>(entity);
        return joiningRequestResponse;
    }
}
