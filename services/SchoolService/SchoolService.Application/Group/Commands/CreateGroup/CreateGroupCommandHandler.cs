using Exception = System.Exception;

namespace SchoolService.Application.Group.Commands.CreateGroup;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Either<GroupModelResponse, Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly ICommandContext _commandContext;

    private readonly IMapper _mapper;

    public CreateGroupCommandHandler(ICommandContext commandContext, ISchoolProfileManager schoolProfileManager, IMapper mapper)
    {
        _commandContext = commandContext;
        _schoolProfileManager = schoolProfileManager;
        _mapper = mapper;
    }

    public async Task<Either<GroupModelResponse, Error>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null || profile.Type != SchoolProfileType.SchoolAdmin)
            return new InvalidError("school_profile");

        var school = await _commandContext.Schools.FindAsync(profile.SchoolId);
        if (school == null)
            return new InvalidError("school_id");

        try
        {
            var existedEntity = await _commandContext.Groups
                .AsNoTracking()
                .Where(group => group.Name.ToLower().Contains(request.Name.ToLower())
                                && group.SchoolId == school.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (existedEntity is not null)
                return new AlreadyExistsError("group");
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the group with values {@Request}.", request);
        }

        var entity = _mapper.Map<Domain.Entities.Group>(request);
        entity.School = school;

        await _commandContext.Groups.AddAsync(entity, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the group with values {@Request}.", request);
            return new InvalidDatabaseOperationError("group");
        }

        var groupModelResponse = _mapper.Map<GroupModelResponse>(entity);
        return groupModelResponse;
    }
}
