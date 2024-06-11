namespace SchoolService.Application.StudyPeriod.Commands.CreateStudyPeriod;

public class CreateStudyPeriodCommandValidator : AbstractValidator<CreateStudyPeriodCommand>
{
    public CreateStudyPeriodCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty)
            .LessThanOrEqualTo(x => x.EndDate);

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty);

    }
}
