namespace SchoolManagementService.Core.Domain;

public abstract class Entity
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }
}
