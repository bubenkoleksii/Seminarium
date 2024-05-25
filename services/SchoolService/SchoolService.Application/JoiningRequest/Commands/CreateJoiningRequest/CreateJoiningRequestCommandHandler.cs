namespace SchoolService.Application.JoiningRequest.Commands.CreateJoiningRequest;

public class CreateJoiningRequestCommandHandler : IRequestHandler<CreateJoiningRequestCommand, Either<JoiningRequestModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IMailService _mailService;

    private readonly IMapper _mapper;

    public CreateJoiningRequestCommandHandler(ICommandContext commandContext, IMailService mailService, IMapper mapper)
    {
        _commandContext = commandContext;
        _mailService = mailService;
        _mapper = mapper;
    }

    public async Task<Either<JoiningRequestModelResponse, Error>> Handle(CreateJoiningRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.JoiningRequest>(request);

        var existedEntity = await _commandContext.JoiningRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RequesterEmail == request.RequesterEmail || r.RegisterCode == request.RegisterCode,
                cancellationToken: cancellationToken);

        if (existedEntity != null && existedEntity.Status != JoiningRequestStatus.Rejected)
            return new AlreadyExistsError("joining request");

        await _commandContext.JoiningRequests.AddAsync(entity, cancellationToken);
        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the joining request with values {@Request}.", request);

            return new InvalidDatabaseOperationError("joining request");
        }

        try
        {
            await _mailService.SendAsync(
                request.RequesterEmail,
                EmailTemplates.CreateJoiningRequest.Subject,
                EmailTemplates.CreateJoiningRequest.GetTemplate(entity.Id, request.Name));
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while send email to {@Email} after creating joining request.", request.RequesterEmail);
        }

        var joiningRequestResponse = _mapper.Map<JoiningRequestModelResponse>(entity);
        return joiningRequestResponse;
    }
}
