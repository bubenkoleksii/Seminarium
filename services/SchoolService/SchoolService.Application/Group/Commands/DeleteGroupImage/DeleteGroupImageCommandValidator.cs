namespace SchoolService.Application.Group.Commands.DeleteGroupImage;

public class DeleteGroupImageCommandValidator : AbstractValidator<DeleteGroupImageCommand>
{
    public DeleteGroupImageCommandValidator()
    {
        RuleFor(x => x.GroupId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
