namespace SchoolManagementService.Core.Domain;

public abstract class Entity
{
    public Guid Id { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }
}
