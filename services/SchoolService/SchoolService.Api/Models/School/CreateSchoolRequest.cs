namespace SchoolService.Api.Models.School;

public record CreateSchoolRequest(
    Guid JoiningRequestId,

    ulong RegisterCode,

    string Name,

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

