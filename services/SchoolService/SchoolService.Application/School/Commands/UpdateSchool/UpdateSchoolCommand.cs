namespace SchoolService.Application.School.Commands.UpdateSchool;

public record UpdateSchoolCommand(Guid Id,

    ulong RegisterCode,

    string Name,

    string? ShortName,

    uint GradingSystem,

    string? Email,

    string? Phone,

    string Type,

    ulong PostalCode,

    string OwnershipType,

    uint StudentsQuantity,

    string Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied,

    string? SiteUrl,

    Stream? Img
) : IRequest<Either<SchoolModelResponse, Error>>;
