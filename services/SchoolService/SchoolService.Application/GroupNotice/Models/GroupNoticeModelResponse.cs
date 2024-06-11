namespace SchoolService.Application.GroupNotice.Models;

public class GroupNoticeModelResponse
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public required string Title { get; set; }

    public string? Text { get; set; }

    public bool IsCrucial { get; set; }

    public Guid GroupId { get; set; }

    public Guid? AuthorId { get; set; }

    public SchoolProfileModelResponse? Author { get; set; }
}
