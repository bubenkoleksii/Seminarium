namespace SchoolService.Application.School.Commands.UpdateSchool;

public class UpdateSchoolCommand : IRequest<Either<SchoolModelResponse, Error>>
{
    public Guid Id { get; init; }

    public Guid? UserId { get; set; }

    public required string RegisterCode { get; init; }

    public required string Name { get; init; }

    public string? ShortName { get; init; }

    public uint GradingSystem { get; init; }

    public string? Email { get; init; }

    public string? Phone { get; init; }

    public required string Type { get; init; }

    public required string PostalCode { get; init; }

    public required string OwnershipType { get; init; }

    public uint StudentsQuantity { get; init; }

    public required string Region { get; init; }

    public string? TerritorialCommunity { get; init; }

    public string? Address { get; init; }

    public bool AreOccupied { get; init; }

    public string? SiteUrl { get; init; }
}

