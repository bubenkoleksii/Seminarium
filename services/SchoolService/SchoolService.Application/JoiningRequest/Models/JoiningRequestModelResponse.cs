namespace SchoolService.Application.JoiningRequest.Models;

public record JoiningRequestModelResponse(
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

    SchoolType Type,

    string PostalCode,

    SchoolOwnershipType OwnershipType,

    uint StudentsQuantity,

    SchoolRegion Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied,

    JoiningRequestStatus Status
);
