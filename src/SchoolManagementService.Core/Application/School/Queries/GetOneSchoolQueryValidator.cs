namespace SchoolManagementService.Core.Application.School.Queries;

public class GetOneSchoolQueryValidator : AbstractValidator<GetOneSchoolQuery>
{
    public GetOneSchoolQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEqual(Guid.Empty);
    }
}
