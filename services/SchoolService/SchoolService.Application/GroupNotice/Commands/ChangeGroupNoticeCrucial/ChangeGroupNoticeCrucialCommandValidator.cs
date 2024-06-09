namespace SchoolService.Application.GroupNotice.Commands.ChangeGroupNoticeCrucial;

public class ChangeGroupNoticeCrucialCommandValidator : AbstractValidator<ChangeGroupNoticeCrucialCommand>
{
    public ChangeGroupNoticeCrucialCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.Id)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
