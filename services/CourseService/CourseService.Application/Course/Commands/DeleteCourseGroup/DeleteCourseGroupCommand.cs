namespace CourseService.Application.Course.Commands.DeleteCourseGroup;

public record DeleteCourseGroupCommand(
    Guid UserId,

    Guid Id
) : IRequest<Option<Error>>;
