using SchoolManagementService.Domain.Enums.School;

namespace SchoolManagementService.Domain.Entities;

public class School : Entity
{
    public long RegisterCode { get; set; }

    public required string Name { get; set; }

    public string? ShortName { get; set; }

    public int GradingSystem { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public SchoolType Type { get; set; }

    public long PostalCode { get; set; }

    public SchoolOwnershipType OwnershipType { get; set; }

    public int StudentsQuantity { get; set; }

    public SchoolRegion Region { get; set; }

    public required string TerritorialCommunity { get; set; }

    public required string Address { get; set; }

    public bool AreOccupied { get; set; }

    public string? SiteUrl { get; set; }

    public string? Img { get; set; }
}