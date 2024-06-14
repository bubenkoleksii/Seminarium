namespace CourseService.Application.LessonItem.Commands.PracticalLessonItem.DeletePracticalLessonItem;

public class DeletePracticalLessonItemCommandValidator : AbstractValidator<DeletePracticalLessonItemCommand>
{
    public DeletePracticalLessonItemCommandValidator()
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
