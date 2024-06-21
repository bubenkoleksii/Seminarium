namespace CourseService.Application.Lesson.Commands.DeleteLesson;

public record DeleteLessonCommand(
    Guid Id,

    Guid UserId
) : IRequest<Option<Error>>;
