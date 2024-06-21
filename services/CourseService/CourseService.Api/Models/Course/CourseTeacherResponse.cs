namespace CourseService.Api.Models.Course;

public record CourseTeacherResponse(
    Guid Id,

    string Name,

    bool IsCreator
);
