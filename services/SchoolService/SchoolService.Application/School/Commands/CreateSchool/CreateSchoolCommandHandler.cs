namespace SchoolService.Application.School.Commands.CreateSchool;

public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, Either<SchoolModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IMapper _mapper;

    public CreateSchoolCommandHandler(ICommandContext commandContext, IMapper mapper)
    {
        _commandContext = commandContext;
        _mapper = mapper;
    }

    public async Task<Either<SchoolModelResponse, Error>> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.School>(request);

        var joiningRequest = await _commandContext.JoiningRequests.FindAsync(request.JoiningRequestId);
        if (joiningRequest is null)
            return new InvalidError("joining request");

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

        var schoolResponse = _mapper.Map<SchoolModelResponse>(entity);
        return schoolResponse;
    }
}
