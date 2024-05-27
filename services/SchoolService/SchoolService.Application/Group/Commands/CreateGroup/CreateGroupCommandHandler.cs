using Exception = System.Exception;

namespace SchoolService.Application.Group.Commands.CreateGroup;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Either<GroupModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IMapper _mapper;

    public CreateGroupCommandHandler(ICommandContext commandContext, IMapper mapper)
    {
        _commandContext = commandContext;
        _mapper = mapper;
    }

    public async Task<Either<GroupModelResponse, Error>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var school = await _commandContext.Schools.FindAsync(request.SchoolId);
        if (school == null)
            return new InvalidError("school_id");

        try
        {
            var existedEntity = await _commandContext.Groups
                .AsNoTracking()
                .Where(group => group.Name.ToLower().Contains(request.Name.ToLower())
                                && group.SchoolId == request.SchoolId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (existedEntity is not null)
                return new AlreadyExistsError("group");
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the group with values {@Request}.", request);
        }

        var entity = _mapper.Map<Domain.Entities.Group>(request);

        await _commandContext.Groups.AddAsync(entity, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the group with values {@Request}.", request);
            return new InvalidDatabaseOperationError("joining request");
        }

        var groupModelResponse = _mapper.Map<GroupModelResponse>(entity);
        return groupModelResponse;
    }
}
