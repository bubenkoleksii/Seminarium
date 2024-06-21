namespace CourseService.Application.Course.Commands.DeleteCourse;

public record DeleteCourseCommand(
    Guid Id,

    Guid UserId
) : IRequest<Option<Error>>;
