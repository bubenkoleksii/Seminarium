namespace CourseService.Application.LessonItem.Commands.PracticalLessonItem.DeletePracticalLessonItem;

public record DeletePracticalLessonItemCommand(
    Guid Id,

    Guid UserId
) : IRequest<Option<Error>>;