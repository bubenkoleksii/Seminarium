namespace SchoolService.Application.School.Commands.UpdateSchool;

public class UpdateSchoolCommand : IRequest<Either<SchoolModelResponse, Error>>
{
    public Guid Id { get; init; }
    public ulong RegisterCode { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? ShortName { get; init; }
    public uint GradingSystem { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string Type { get; init; } = string.Empty;
    public ulong PostalCode { get; init; }
    public string OwnershipType { get; init; } = string.Empty;
    public uint StudentsQuantity { get; init; }
    public string Region { get; init; } = string.Empty;
    public string? TerritorialCommunity { get; init; }
    public string? Address { get; init; }
    public bool AreOccupied { get; init; }
    public string? SiteUrl { get; init; }
}

