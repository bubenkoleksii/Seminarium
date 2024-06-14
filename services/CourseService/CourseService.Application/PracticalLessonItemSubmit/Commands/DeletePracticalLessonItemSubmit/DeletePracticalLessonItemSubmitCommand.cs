namespace CourseService.Application.PracticalLessonItemSubmit.Commands.DeletePracticalLessonItemSubmit;

public record DeletePracticalLessonItemSubmitCommand(
    Guid Id,

    Guid UserId
) : IRequest<Option<Error>>;