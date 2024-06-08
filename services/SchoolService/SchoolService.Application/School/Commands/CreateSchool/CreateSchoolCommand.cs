namespace SchoolService.Application.School.Commands.CreateSchool;

public record CreateSchoolCommand(
    Guid JoiningRequestId,

    string RegisterCode,

    string Name,

    string? ShortName,

    uint GradingSystem,

    SchoolType Type,

    string PostalCode,

    SchoolOwnershipType OwnershipType,

    uint StudentsQuantity,

    SchoolRegion Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied = false
) : IRequest<Either<SchoolModelResponse, Error>>;
