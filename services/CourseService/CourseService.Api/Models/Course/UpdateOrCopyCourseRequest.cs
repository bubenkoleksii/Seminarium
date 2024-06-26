namespace CourseService.Api.Models.Course;

public record UpdateOrCopyCourseRequest(
    Guid Id,

    Guid StudyPeriodId,

    string Name,

    string? Description
);
