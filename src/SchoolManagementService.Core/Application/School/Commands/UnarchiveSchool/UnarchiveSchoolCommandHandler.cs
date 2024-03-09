using SchoolManagementService.Core.Application.Common.DataContext;
using SchoolManagementService.Core.Domain.Errors;
using SchoolManagementService.Core.Domain.Errors.AlreadyExists;
using SchoolManagementService.Core.Domain.Errors.NotFound;

namespace SchoolManagementService.Core.Application.School.Commands.UnarchiveSchool;

public class UnarchiveSchoolCommandHandler : IRequestHandler<UnarchiveSchoolCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    public UnarchiveSchoolCommandHandler(ICommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    public async Task<Option<Error>> Handle(UnarchiveSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Schools
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.Id, "school");

        if (!entity.IsArchived)
            return new AlreadyUnarchived(request.Id, "school");

        entity.IsArchived = false;
        await _commandContext.SaveChangesAsync(cancellationToken);

        return Option<Error>.None;
    }
}
