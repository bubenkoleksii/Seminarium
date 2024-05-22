namespace SchoolService.Application.School.Commands.CreateSchool;

public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, Either<SchoolModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IMailService _mailService;

    private readonly IMapper _mapper;

    public CreateSchoolCommandHandler(ICommandContext commandContext, IMailService mailService, IMapper mapper)
    {
        _commandContext = commandContext;
        _mailService = mailService;
        _mapper = mapper;
    }

    public async Task<Either<SchoolModelResponse, Error>> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.School>(request);

        var joiningRequest = await _commandContext.JoiningRequests.FindAsync(request.JoiningRequestId);
        if (joiningRequest is null)
            return new InvalidError("joining request");

        joiningRequest.Status = JoiningRequestStatus.Approved;
        entity.JoiningRequest = joiningRequest;

        var isAlreadyExists = UniquenessChecker.GetErrorIfAlreadyExists(_commandContext, entity, out var error);
        if (isAlreadyExists)
            return error!;

        await _commandContext.Schools.AddAsync(entity, cancellationToken);
        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the school with values {@Request}.", request);

            return new InvalidDatabaseOperationError("school");
        }

        try
        {
            await _mailService.SendAsync(
                joiningRequest.RequesterEmail,
                EmailTemplates.AcceptJoiningRequest.Subject,
                EmailTemplates.AcceptJoiningRequest.GetTemplate(joiningRequest.Id, entity.Name));
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while send email to {@Email} after accepting joining request.", joiningRequest.RequesterEmail);
        }

        var schoolResponse = _mapper.Map<SchoolModelResponse>(entity);
        return schoolResponse;
    }
}
