namespace CourseService.Application.Course.Commands.DeleteCourseGroup;

public record DeleteCourseGroupCommand(
    Guid UserId,

    Guid Id,

    Guid CourseId
) : IRequest<Option<Error>>;
