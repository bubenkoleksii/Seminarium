namespace SchoolService.Application.Group.Commands.DeleteGroupImage;

public record DeleteGroupImageCommand(Guid GroupId, Guid UserId) : IRequest<Option<Error>>;
