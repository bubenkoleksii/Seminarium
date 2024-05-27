namespace SchoolService.Application.Group.Models;

public record GroupModelResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Name,

    byte StudyPeriodNumber,

    string? Img
);
