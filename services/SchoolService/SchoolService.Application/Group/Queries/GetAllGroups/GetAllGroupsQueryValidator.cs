namespace SchoolService.Application.Group.Queries.GetAllGroups;

public class GetAllGroupsQueryValidator : AbstractValidator<GetAllGroupsQuery>
{
    public GetAllGroupsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.StudyPeriodNumber)
            .GreaterThanOrEqualTo((byte)0)
            .WithErrorCode(ErrorTitles.Common.LowerZero);

        RuleFor(x => x.Skip)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .GreaterThanOrEqualTo((uint)0)
            .WithErrorCode(ErrorTitles.Common.LowerZero);

        RuleFor(x => x.Take)
            .GreaterThanOrEqualTo((uint)0)
            .WithErrorCode(ErrorTitles.Common.LowerZero);
    }
}
