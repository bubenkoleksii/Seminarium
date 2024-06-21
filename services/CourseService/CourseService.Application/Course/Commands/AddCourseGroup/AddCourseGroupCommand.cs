namespace CourseService.Application.Course.Commands.AddCourseGroup;

public record AddCourseGroupCommand(
    Guid UserId,

    string Name,

    Guid CourseId,

    Guid SchoolId
) : IRequest<Either<CourseGroupModelResponse, Error>>;
