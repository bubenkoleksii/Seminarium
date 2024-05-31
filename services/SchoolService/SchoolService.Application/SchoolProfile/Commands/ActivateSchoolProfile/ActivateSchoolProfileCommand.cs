namespace SchoolService.Application.SchoolProfile.Commands.ActivateSchoolProfile;

public record ActivateSchoolProfileCommand(
    Guid Id,

    Guid UserId
) : IRequest<Either<SchoolProfileModelResponse, Error>>;
