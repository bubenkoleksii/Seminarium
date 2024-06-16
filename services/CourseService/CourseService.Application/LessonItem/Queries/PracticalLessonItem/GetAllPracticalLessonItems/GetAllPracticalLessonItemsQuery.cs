namespace CourseService.Application.LessonItem.Queries.PracticalLessonItem.GetAllPracticalLessonItems;

public record GetAllPracticalLessonItemsQuery(Guid LessonId, Guid UserId)
    : IRequest<IEnumerable<PracticalLessonItemModelResponse>>;
