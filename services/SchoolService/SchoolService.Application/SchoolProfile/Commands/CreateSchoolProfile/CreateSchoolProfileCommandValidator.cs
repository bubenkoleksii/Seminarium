namespace SchoolService.Application.SchoolProfile.Commands.CreateSchoolProfile;

public class CreateSchoolProfileCommandValidator : AbstractValidator<CreateSchoolProfileCommand>
{
    public CreateSchoolProfileCommandValidator()
    {
        RuleFor(x => x.InvitationCode)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
