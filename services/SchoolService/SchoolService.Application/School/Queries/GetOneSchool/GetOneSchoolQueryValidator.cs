namespace SchoolService.Application.School.Queries.GetOneSchool;

public class GetOneSchoolQueryValidator : AbstractValidator<GetOneSchoolQuery>
{
    public GetOneSchoolQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
