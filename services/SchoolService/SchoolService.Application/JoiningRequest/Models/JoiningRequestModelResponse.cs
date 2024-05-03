using SchoolService.Domain.Enums.JoiningRequest;

namespace SchoolService.Application.JoiningRequest.Models;

public record JoiningRequestModelResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    ulong RegisterCode,

    string Name,

    string RequesterEmail,

    string RequesterPhone,

    string RequesterFullName,

    string? ShortName,

    uint GradingSystem,

    SchoolType Type,

    ulong PostalCode,

    SchoolOwnershipType OwnershipType,

    uint StudentsQuantity,

    SchoolRegion Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied,

    JoiningRequestStatus Status
);
