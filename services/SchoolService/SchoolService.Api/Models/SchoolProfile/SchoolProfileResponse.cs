namespace SchoolService.Api.Models.SchoolProfile;

public record SchoolProfileResponse(
    Guid Id,

    Guid UserId,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Type,

    string? Phone,

    string? Img,

    string? Details,

    string? Data,

    Guid? SchoolId
);
