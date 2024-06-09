namespace SchoolService.Application.GroupNotice.Commands.DeleteGroupNotice;

public record DeleteGroupNoticeCommand(
    Guid Id,

    Guid UserId
) : IRequest<Option<Error>>;
