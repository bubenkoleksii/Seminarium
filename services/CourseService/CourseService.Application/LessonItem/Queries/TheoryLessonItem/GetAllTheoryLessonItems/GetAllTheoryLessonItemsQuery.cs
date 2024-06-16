namespace CourseService.Application.LessonItem.Queries.TheoryLessonItem.GetAllTheoryLessonItems;

public record GetAllTheoryLessonItemsQuery(Guid LessonId, Guid UserId)
    : IRequest<IEnumerable<TheoryLessonItemModelResponse>>;