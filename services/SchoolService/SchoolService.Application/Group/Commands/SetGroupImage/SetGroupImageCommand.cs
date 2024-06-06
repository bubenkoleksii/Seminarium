namespace SchoolService.Application.Group.Commands.SetGroupImage;

public record SetGroupImageCommand(
    Guid GroupId,

    Guid UserId,

    string Name,

    Stream Stream,

    int? UrlExpirationInMin
) : IRequest<Either<FileSuccess, Error>>;
