namespace SchoolService.Application.StudyPeriod.Commands.UpdateStudyPeriod;

public class UpdateStudyPeriodCommandValidator : AbstractValidator<UpdateStudyPeriodCommand>
{
    public UpdateStudyPeriodCommandValidator()
    {
        RuleFor(x => x.Id)
             .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

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
