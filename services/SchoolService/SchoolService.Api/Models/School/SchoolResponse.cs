namespace SchoolService.Api.Models.School;

public record SchoolResponse(
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

    string Type,

    string PostalCode,

    string OwnershipType,

    uint StudentsQuantity,

    string Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied,

    string? SiteUrl,

    string? Img
);
