using SchoolManagementService.Core.Application.DataContext;

namespace SchoolManagementService.Core.Application.School.Commands.CreateSchool;

public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, Guid>
{
    private readonly ICommandContext _commandContext;

    public CreateSchoolCommandHandler(ICommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    public Task<Guid> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private Domain.School GenerateSchool(CreateSchoolCommand request)
    {
        throw new NotImplementedException();
    }
}
