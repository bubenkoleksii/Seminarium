namespace SchoolService.Application.School.Commands.SetSchoolImage;

public class SetSchoolImageCommandValidator : AbstractValidator<SetSchoolImageCommand>
{
    public SetSchoolImageCommandValidator()
    {
        RuleFor(x => x.SchoolId)
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
