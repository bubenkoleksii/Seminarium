namespace CourseService.Application.PracticalLessonItemSubmit.Commands.UpdatePracticalLessonItemSubmit;

public class UpdatePracticalLessonItemSubmitCommandValidator : AbstractValidator<UpdatePracticalLessonItemSubmitCommand>
{
    public UpdatePracticalLessonItemSubmitCommandValidator()
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
            .MaximumLength(2048)
            .WithErrorCode(ErrorTitles.Common.TooLong);
    }
}
