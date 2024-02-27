using SchoolManagementService.Core.Domain.Enums.School;

namespace SchoolManagementService.Core.Domain.Entities;

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

    public required string TerritorialCommunity { get; set; }

    public required string Address { get; set; }

    public bool AreOccupied { get; set; }

    public string? SiteUrl { get; set; }

    public string? Img { get; set; }
}
