namespace CourseService.Application.LessonItem.Queries.PracticalLessonItem.GetAllPracticalLessonItems;

public class GetAllPracticalLessonItemsQueryValidator : AbstractValidator<GetAllPracticalLessonItemsQuery>
{
    public GetAllPracticalLessonItemsQueryValidator()
    {
        RuleFor(x => x.LessonId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
