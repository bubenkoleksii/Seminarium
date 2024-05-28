namespace SchoolService.Application.SchoolProfile.Models;

public record SchoolProfileModelResponse(
    Guid Id,

    Guid UserId,

    bool IsActive,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Type,

    string? Phone,

    string? Img,

    string? Details,

    string? Data,

    Guid? SchoolId,

    Guid? GroupId,

    Guid? ClassTeacherGroupId
);
