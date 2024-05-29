namespace SchoolService.Application.SchoolProfile.Commands.SetSchoolProfileImage;

public class SetSchoolProfileImageCommandValidator : AbstractValidator<SetSchoolProfileImageCommand>
{
    public SetSchoolProfileImageCommandValidator()
    {
        RuleFor(x => x.SchoolProfileId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.UserId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.Stream)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Stream.Null)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
