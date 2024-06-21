namespace CourseService.Api.Models.Course;

public record CreateCourseRequest(
    Guid StudyPeriodId,

    string Name,

    string? Description
);
