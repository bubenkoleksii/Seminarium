namespace SchoolService.Application.Group.Commands.SetGroupImage;

public record SetGroupImageCommand(
    Guid GroupId,

    string Name,

    Stream Stream,

    int? UrlExpirationInMin
) : IRequest<Either<FileSuccess, Error>>;
