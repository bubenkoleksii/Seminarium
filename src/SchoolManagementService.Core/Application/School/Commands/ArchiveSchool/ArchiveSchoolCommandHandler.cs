using SchoolManagementService.Core.Application.Common.DataContext;
using SchoolManagementService.Core.Domain.Errors;
using SchoolManagementService.Core.Domain.Errors.NotFound;

namespace SchoolManagementService.Core.Application.School.Commands.ArchiveSchool;

public class ArchiveSchoolCommandHandler : IRequestHandler<ArchiveSchoolCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    public ArchiveSchoolCommandHandler(ICommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    public async Task<Option<Error>> Handle(ArchiveSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Schools.FindAsync(request.Id);

        if (entity is null)
            return new NotFoundByIdError(request.Id, "school");

        entity.IsArchived = true;
        await _commandContext.SaveChangesAsync(cancellationToken);

        return Option<Error>.None;
    }
}
