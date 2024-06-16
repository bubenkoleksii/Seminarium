namespace CourseService.Application.LessonItem.Queries.TheoryLessonItem.GetAllTheoryLessonItems;

public class GetAllTheoryLessonItemsQueryValidator : AbstractValidator<GetAllTheoryLessonItemsQuery>
{
    public GetAllTheoryLessonItemsQueryValidator()
    {
        RuleFor(x => x.LessonId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
