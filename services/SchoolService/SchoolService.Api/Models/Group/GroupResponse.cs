namespace SchoolService.Api.Models.Group;

public record GroupResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Name,

    byte StudyPeriodNumber,

    string? Img
);
