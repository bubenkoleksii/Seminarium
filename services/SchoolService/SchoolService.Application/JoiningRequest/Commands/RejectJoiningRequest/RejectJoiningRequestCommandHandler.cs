namespace SchoolService.Application.JoiningRequest.Commands.RejectJoiningRequest;

public class RejectJoiningRequestCommandHandler : IRequestHandler<RejectJoiningRequestCommand, Either<RejectJoiningRequestModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IMailService _mailService;

    private readonly IMapper _mapper;

    public RejectJoiningRequestCommandHandler(ICommandContext commandContext, IMailService mailService, IMapper mapper)
    {
        _commandContext = commandContext;
        _mailService = mailService;
        _mapper = mapper;
    }

    public async Task<Either<RejectJoiningRequestModelResponse, Error>> Handle(RejectJoiningRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.JoiningRequests
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: CancellationToken.None);

        if (entity is null)
            return new NotFoundByIdError(request.Id, "joining request");

        entity.Status = JoiningRequestStatus.Rejected;
        _commandContext.JoiningRequests.Update(entity);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while setting status to rejected of the joining request with values {@Request}.", request);

            return new InvalidDatabaseOperationError("joining request");
        }

        try
        {
            await _mailService.SendAsync(
                entity.RequesterEmail,
                EmailTemplates.RejectJoiningRequest.Subject,
                EmailTemplates.RejectJoiningRequest.GetTemplate(entity.Id, entity.Name, request.Text));
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while send email to {@Email} after rejecting joining request.", entity.RequesterEmail);
        }

        var rejectJoiningRequestResponse = _mapper.Map<RejectJoiningRequestModelResponse>(entity);
        return rejectJoiningRequestResponse;
    }
}
