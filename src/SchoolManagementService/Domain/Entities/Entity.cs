namespace SchoolManagementService.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }
}