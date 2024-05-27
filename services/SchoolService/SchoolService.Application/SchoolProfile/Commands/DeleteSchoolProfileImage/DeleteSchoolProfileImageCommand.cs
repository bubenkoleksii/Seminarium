namespace SchoolService.Application.SchoolProfile.Commands.DeleteSchoolProfileImage;

public record DeleteSchoolProfileImageCommand(Guid SchoolProfileId) : IRequest<Option<Error>>;
