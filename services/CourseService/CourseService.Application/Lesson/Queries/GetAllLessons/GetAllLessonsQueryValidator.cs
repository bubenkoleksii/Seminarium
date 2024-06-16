namespace CourseService.Application.Lesson.Queries.GetAllLessons;

public class GetAllLessonsQueryValidator : AbstractValidator<GetAllLessonsQuery>
{
    public GetAllLessonsQueryValidator()
    {
        RuleFor(x => x.CourseId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
