namespace SchoolService.Domain.Entities;

public class GroupNotice : Entity
{
    public required string Title { get; set; }

    public string? Text { get; set; }

    public bool IsCrucial { get; set; }

    public Guid GroupId { get; set; }

    public required Group Group { get; set; }

    public Guid? AuthorId { get; set; }

    public SchoolProfile? Author { get; set; }
}
