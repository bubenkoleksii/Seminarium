namespace SchoolService.Domain.Entities;

public class ParentChild
{
    public Guid Id { get; set; }

    public Guid? ParentId { get; set; }

    public SchoolProfile? Parent { get; set; }

    public Guid? ChildId { get; set; }

    public SchoolProfile? Child { get; set; }
}
