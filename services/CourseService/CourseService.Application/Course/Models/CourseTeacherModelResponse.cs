namespace CourseService.Application.Course.Models;

public record CourseTeacherModelResponse(
    Guid Id,

    string? Name,

    bool IsCreator
);
