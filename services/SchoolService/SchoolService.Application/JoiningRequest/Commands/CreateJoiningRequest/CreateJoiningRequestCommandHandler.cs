namespace SchoolService.Application.JoiningRequest.Commands.CreateJoiningRequest;

public class CreateJoiningRequestCommandHandler : IRequestHandler<CreateJoiningRequestCommand, Either<JoiningRequestModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IMapper _mapper;

    public CreateJoiningRequestCommandHandler(ICommandContext commandContext, IMapper mapper)
    {
        _commandContext = commandContext;
        _mapper = mapper;
    }

    public async Task<Either<JoiningRequestModelResponse, Error>> Handle(CreateJoiningRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.JoiningRequest>(request);

        var existedEntity = await _commandContext.JoiningRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RequesterEmail == request.RequesterEmail || r.RegisterCode == request.RegisterCode,
                cancellationToken: cancellationToken);

        if (existedEntity != null)
            return new AlreadyExistsError("joining request");

        await _commandContext.JoiningRequests.AddAsync(entity, cancellationToken);
        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the school with values {@Request}.", request);

            return new InvalidDatabaseOperationError("joining request");
        }

        var joiningRequestResponse = _mapper.Map<JoiningRequestModelResponse>(entity);
        return joiningRequestResponse;
    }
}
