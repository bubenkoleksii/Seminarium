namespace SchoolService.Application.Group.Models;

public record GroupModelResponse(
    Guid Id,

    Guid SchoolId,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Name,

    byte StudyPeriodNumber,

    string? Img
);
