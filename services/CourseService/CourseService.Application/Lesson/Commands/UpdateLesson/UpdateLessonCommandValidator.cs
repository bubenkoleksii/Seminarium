namespace CourseService.Application.Lesson.Commands.UpdateLesson;

public class UpdateLessonCommandValidator : AbstractValidator<UpdateLessonCommand>
{
    public UpdateLessonCommandValidator()
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

        RuleFor(x => x.CourseId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.Topic)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty)
            .MaximumLength(250)
            .WithErrorCode(ErrorTitles.Common.TooLong);

        RuleFor(x => x.Homework)
            .MaximumLength(1024)
            .WithErrorCode(ErrorTitles.Common.TooLong);
    }
}
