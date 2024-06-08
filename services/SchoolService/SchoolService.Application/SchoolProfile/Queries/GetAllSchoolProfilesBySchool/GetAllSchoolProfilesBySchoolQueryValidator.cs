namespace SchoolService.Application.SchoolProfile.Queries.GetAllSchoolProfilesBySchool;

public class GetAllSchoolProfilesBySchoolQueryValidator : AbstractValidator<GetAllSchoolProfilesBySchoolQuery>
{
    public GetAllSchoolProfilesBySchoolQueryValidator()
    {
        RuleFor(x => x.UserId)
        .NotNull()
        .WithErrorCode(ErrorTitles.Common.Null)
        .NotEqual(Guid.Empty)
        .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
