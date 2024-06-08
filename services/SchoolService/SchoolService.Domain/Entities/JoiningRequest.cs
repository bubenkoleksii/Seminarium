namespace SchoolService.Domain.Entities;

public class JoiningRequest : Entity
{
    public required string RegisterCode { get; set; }

    public required string RequesterEmail { get; set; }

    public required string RequesterPhone { get; set; }

    public required string RequesterFullName { get; set; }

    public required string Name { get; set; }

    public string? ShortName { get; set; }

    public uint GradingSystem { get; set; }

    public SchoolType Type { get; set; }

    public required string PostalCode { get; set; }

    public SchoolOwnershipType OwnershipType { get; set; }

    public uint StudentsQuantity { get; set; }

    public SchoolRegion Region { get; set; }

    public string? TerritorialCommunity { get; set; }

    public string? Address { get; set; }

    public bool AreOccupied { get; set; }

    public JoiningRequestStatus Status { get; set; }

    public Guid? SchoolId { get; set; }

    public School? School { get; set; }
}
