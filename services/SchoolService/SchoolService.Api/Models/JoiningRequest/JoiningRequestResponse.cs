namespace SchoolService.Api.Models.JoiningRequest;

public record JoiningRequestResponse(
    Guid Id,

    Guid? SchoolId,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string RegisterCode,

    string Name,

    string RequesterEmail,

    string RequesterPhone,

    string RequesterFullName,

    string? ShortName,

    uint GradingSystem,

    string Type,

    string PostalCode,

    string OwnershipType,

    uint StudentsQuantity,

    string Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied,

    string Status
);
