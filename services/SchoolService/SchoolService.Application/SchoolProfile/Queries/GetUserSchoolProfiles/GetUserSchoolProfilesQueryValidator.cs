namespace SchoolService.Application.SchoolProfile.Queries.GetUserSchoolProfiles;

public class GetUserSchoolProfilesQueryValidator : AbstractValidator<GetUserSchoolProfilesQuery>
{
    public GetUserSchoolProfilesQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
