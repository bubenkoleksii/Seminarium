namespace CourseService.Application.Course.Commands.DeleteCourseTeacher;

public record DeleteCourseTeacherCommand(
    Guid Id,

    Guid UserId
) : IRequest<Option<Error>>;
