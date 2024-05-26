namespace SchoolService.Application.SchoolProfile.Commands.CreateSchoolProfile;

public record CreateSchoolProfileCommand(
    string InvitationCode
) : IRequest<Either<SchoolProfileModelResponse, Error>>;
