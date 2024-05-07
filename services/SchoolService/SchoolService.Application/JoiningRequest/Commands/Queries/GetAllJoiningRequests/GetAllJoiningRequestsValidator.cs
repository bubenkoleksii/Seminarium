namespace SchoolService.Application.JoiningRequest.Commands.Queries.GetAllJoiningRequests;

public class GetAllJoiningRequestsValidator : AbstractValidator<GetAllJoiningRequestsQuery>
{
    public GetAllJoiningRequestsValidator()
    {
        RuleFor(x => x.SchoolName)
            .MaximumLength(250)
            .WithErrorCode(ErrorTitles.Common.TooLong);
    }
}
