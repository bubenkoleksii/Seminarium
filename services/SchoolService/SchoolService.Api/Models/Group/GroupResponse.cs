namespace SchoolService.Api.Models.Group;

public record GroupResponse(
    Guid Id,

    Guid SchoolId,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Name,

    byte StudyPeriodNumber,

    string? Img
);
