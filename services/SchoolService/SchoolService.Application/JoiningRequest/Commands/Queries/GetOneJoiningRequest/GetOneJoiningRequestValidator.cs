namespace SchoolService.Application.JoiningRequest.Commands.Queries.GetOneJoiningRequest;

public class GetOneJoiningRequestValidator : AbstractValidator<GetOneJoiningRequestQuery>
{
    public GetOneJoiningRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
