namespace CourseService.Application.PracticalLessonItemSubmit.Commands.DeletePracticalLessonItemSubmit;

public class DeletePracticalLessonItemSubmitCommandValidator : AbstractValidator<DeletePracticalLessonItemSubmitCommand>
{
    public DeletePracticalLessonItemSubmitCommandValidator()
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
