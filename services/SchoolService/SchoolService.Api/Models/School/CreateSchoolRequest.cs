namespace SchoolService.Api.Models.School;

public record CreateSchoolRequest(
    Guid JoiningRequestId,

    string RegisterCode,

    string Name,

    string? ShortName,

    uint GradingSystem,

    string Type,

    string PostalCode,

    string OwnershipType,

    uint StudentsQuantity,

    string Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied
);

