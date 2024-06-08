namespace SchoolService.Application.School.Models;

public record SchoolModelResponse(
    Guid Id,

    Guid JoiningRequestId,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string RegisterCode,

    string Name,

    string? ShortName,

    uint GradingSystem,

    string? Email,

    string? Phone,

    SchoolType Type,

    string PostalCode,

    SchoolOwnershipType OwnershipType,

    uint StudentsQuantity,

    SchoolRegion Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied,

    string? SiteUrl,

    string? Img
);
