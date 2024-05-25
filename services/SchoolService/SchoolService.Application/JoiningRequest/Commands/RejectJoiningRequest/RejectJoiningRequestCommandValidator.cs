namespace SchoolService.Application.JoiningRequest.Commands.RejectJoiningRequest;

public class RejectJoiningRequestCommandValidator : AbstractValidator<RejectJoiningRequestCommand>
{
    public RejectJoiningRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
