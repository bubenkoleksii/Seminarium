namespace CourseService.Api.Models.Course;

public record CourseResponse(
    Guid Id,

    Guid StudyPeriodId,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Name,

    string? Description,

    IEnumerable<CourseTeacherResponse>? Teachers,

    IEnumerable<CourseGroupResponse>? Groups
);