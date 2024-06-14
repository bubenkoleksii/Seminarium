namespace CourseService.Api.Models.Course;

public record UpdateCourseRequest(
    Guid Id,

    Guid StudyPeriodId,

    string Name,

    string? Description
);
