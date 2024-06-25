namespace CourseService.Application.PracticalLessonItemSubmit.Commands.AddResultsPracticalLessonItemSubmit;

public class AddResultsPracticalLessonItemSubmitCommandValidator : AbstractValidator<AddResultsPracticalLessonItemSubmitCommand>
{
    public AddResultsPracticalLessonItemSubmitCommandValidator()
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

        RuleFor(x => x.Text)
            .MaximumLength(256)
            .WithErrorCode(ErrorTitles.Common.TooLong);
    }
}
