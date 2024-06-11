namespace CourseService.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }

    public bool IsArchived { get; set; }

    public DateTime? LastArchivedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }
}
