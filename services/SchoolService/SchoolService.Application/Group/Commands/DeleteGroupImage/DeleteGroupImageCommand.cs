namespace SchoolService.Application.Group.Commands.DeleteGroupImage;

public record DeleteGroupImageCommand(Guid GroupId) : IRequest<Option<Error>>;
