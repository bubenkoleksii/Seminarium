namespace SchoolService.Application.Group.Commands.SetGroupImage;

public class SetGroupImageCommandValidator : AbstractValidator<SetGroupImageCommand>
{
    public SetGroupImageCommandValidator()
    {
        RuleFor(x => x.GroupId)
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
