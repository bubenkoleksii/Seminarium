namespace CourseService.Api.Models.Course;

public record CourseResponse(
    Guid Id,

    Guid StudyPeriodId,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Name,

    string? Description,

    IEnumerable<SchoolProfileContract>? Teachers,

    IEnumerable<GroupContract>? Groups
);