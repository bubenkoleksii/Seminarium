namespace SchoolService.Application.SchoolProfile.Commands.SetSchoolProfileImage;

public record SetSchoolProfileImageCommand(
    Guid SchoolProfileId,

    string Name,

    Stream Stream,

    int? UrlExpirationInMin
) : IRequest<Either<FileSuccess, Error>>;
