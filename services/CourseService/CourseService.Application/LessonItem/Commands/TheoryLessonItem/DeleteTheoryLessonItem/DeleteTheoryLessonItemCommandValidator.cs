namespace CourseService.Application.LessonItem.Commands.TheoryLessonItem.DeleteTheoryLessonItem;

public class DeleteTheoryLessonItemCommandValidator : AbstractValidator<DeleteTheoryLessonItemCommand>
{
    public DeleteTheoryLessonItemCommandValidator()
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
    }
}
