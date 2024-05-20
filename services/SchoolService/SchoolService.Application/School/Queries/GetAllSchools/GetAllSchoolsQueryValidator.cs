﻿namespace SchoolService.Application.School.Queries.GetAllSchools;

public class GetAllSchoolsQueryValidator : AbstractValidator<GetAllSchoolsQuery>
{
    public GetAllSchoolsQueryValidator()
    {
        RuleFor(x => x.SchoolName)
            .MaximumLength(250)
            .WithErrorCode(ErrorTitles.Common.TooLong);

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
