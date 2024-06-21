namespace CourseService.Application.Course.Commands.AddCourseTeacher;

public record AddCourseTeacherCommand(Guid UserId, string Name, Guid CourseId, Guid SchoolId)
    : IRequest<Either<CourseTeacherModelResponse, Error>>;
