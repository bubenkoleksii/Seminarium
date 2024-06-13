namespace CourseService.Domain.Entities;

public class LessonItem : Entity
{
    public DateTime? Deadline { get; set; }

    public required string Title { get; set; }

    public string? Text { get; set; }

    public Guid? AuthorId { get; set; }

    public CourseTeacher? Author { get; set; }

    public ICollection<Attachment>? Attachments { get; set; }
}
