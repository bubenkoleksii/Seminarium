namespace CourseService.Application.LessonItem.Commands.TheoryLessonItem.UpdateTheoryLessonItem;

public class UpdateTheoryLessonItemCommandValidator : AbstractValidator<UpdateTheoryLessonItemCommand>
{
    public UpdateTheoryLessonItemCommandValidator()
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

        RuleFor(x => x.LessonId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty)
            .MaximumLength(250)
            .WithErrorCode(ErrorTitles.Common.TooLong);

        RuleFor(x => x.Text)
            .MaximumLength(2048)
            .WithErrorCode(ErrorTitles.Common.TooLong);
    }
}
