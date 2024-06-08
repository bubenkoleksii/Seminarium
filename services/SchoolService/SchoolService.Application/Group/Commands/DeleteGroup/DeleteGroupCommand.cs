namespace SchoolService.Application.Group.Commands.DeleteGroup;

public record DeleteGroupCommand(
    Guid Id,

    Guid UserId
) : IRequest<Option<Error>>;
