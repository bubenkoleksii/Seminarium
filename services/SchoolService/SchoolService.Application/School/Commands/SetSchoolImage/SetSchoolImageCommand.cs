namespace SchoolService.Application.School.Commands.SetSchoolImage;

public record SetSchoolImageCommand(
    Guid SchoolId,

    string Name,

    Stream Stream,

    int? UrlExpirationInMin
) : IRequest<Either<FileSuccess, Error>>;
