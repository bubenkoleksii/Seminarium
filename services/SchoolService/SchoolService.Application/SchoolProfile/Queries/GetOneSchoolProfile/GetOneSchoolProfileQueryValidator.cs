namespace SchoolService.Application.SchoolProfile.Queries.GetOneSchoolProfile;

public class GetOneSchoolProfileQueryValidator : AbstractValidator<GetOneSchoolProfileQuery>
{
    public GetOneSchoolProfileQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
