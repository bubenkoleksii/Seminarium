namespace CourseService.Application.LessonItem.Commands.TheoryLessonItem.DeleteTheoryLessonItem;

public record DeleteTheoryLessonItemCommand(
    Guid Id,

    Guid UserId
) : IRequest<Option<Error>>;
