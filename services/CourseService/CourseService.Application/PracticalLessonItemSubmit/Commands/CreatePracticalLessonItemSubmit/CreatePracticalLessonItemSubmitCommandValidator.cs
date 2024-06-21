namespace CourseService.Application.PracticalLessonItemSubmit.Commands.CreatePracticalLessonItemSubmit;

public class CreatePracticalLessonItemSubmitCommandValidator : AbstractValidator<CreatePracticalLessonItemSubmitCommand>
{
    public CreatePracticalLessonItemSubmitCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.PracticalLessonItemId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.Text)
            .MaximumLength(2048)
            .WithErrorCode(ErrorTitles.Common.TooLong);
    }
}
