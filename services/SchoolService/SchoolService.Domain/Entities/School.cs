namespace SchoolService.Domain.Entities;

public class School : Entity
{
    public ulong RegisterCode { get; set; }

    public required string Name { get; set; }

    public string? ShortName { get; set; }

    public uint GradingSystem { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public SchoolType Type { get; set; }

    public ulong PostalCode { get; set; }

    public SchoolOwnershipType OwnershipType { get; set; }

    public uint StudentsQuantity { get; set; }

    public SchoolRegion Region { get; set; }

    public string? TerritorialCommunity { get; set; }

    public string? Address { get; set; }

    public bool AreOccupied { get; set; }

    public string? SiteUrl { get; set; }

    public string? Img { get; set; }

    public Guid JoiningRequestId { get; set; }

    public required JoiningRequest JoiningRequest { get; set; }

    public IEnumerable<SchoolProfile>? Teachers { get; set; }
}
