using SchoolManagementService.Core.Application.DataContext;

namespace SchoolManagementService.Core.Application.School.Commands.CreateSchool;

public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, Guid>
{
    private readonly ICommandContext _commandContext;

    private readonly IMapper _mapper;

    public CreateSchoolCommandHandler(ICommandContext commandContext, IMapper mapper)
    {
        _commandContext = commandContext;
        _mapper = mapper;
    }

    public Task<Guid> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.School>(request);

        var id = Guid.NewGuid();
        return Task.FromResult(id);
    }
}
