namespace CourseService.Application.Course.CreateCourse;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty)
            .MaximumLength(250)
            .WithErrorCode(ErrorTitles.Common.TooLong);

        RuleFor(x => x.Description)
            .MaximumLength(250)
            .WithErrorCode(ErrorTitles.Common.TooLong);
    }
}
