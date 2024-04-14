namespace SchoolService.Api.Models.JoiningRequest;

public record CreateJoiningRequest(
    ulong RegisterCode,

    string Name,

    string RequesterEmail,

    string RequesterPhone,

    string RequesterFullName,

    string? ShortName,

    uint GradingSystem,

    string Type,

    ulong PostalCode,

    string OwnershipType,

    uint StudentsQuantity,

    string Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied
);
