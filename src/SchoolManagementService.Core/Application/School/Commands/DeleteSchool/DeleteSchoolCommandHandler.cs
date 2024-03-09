using SchoolManagementService.Core.Application.Common.DataContext;
using SchoolManagementService.Core.Domain.Errors;
using SchoolManagementService.Core.Domain.Errors.NotFound;

namespace SchoolManagementService.Core.Application.School.Commands.DeleteSchool;

public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    public DeleteSchoolCommandHandler(ICommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    public async Task<Option<Error>> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Schools
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.Id, "school");

        _commandContext.Schools.Remove(entity);
        await _commandContext.SaveChangesAsync(cancellationToken);

        return Option<Error>.None;
    }
}
