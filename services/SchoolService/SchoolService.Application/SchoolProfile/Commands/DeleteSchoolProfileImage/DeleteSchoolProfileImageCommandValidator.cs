namespace SchoolService.Application.SchoolProfile.Commands.DeleteSchoolProfileImage;

public class DeleteSchoolProfileImageCommandValidator : AbstractValidator<DeleteSchoolProfileImageCommand>
{
    public DeleteSchoolProfileImageCommandValidator()
    {
        RuleFor(x => x.SchoolProfileId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
