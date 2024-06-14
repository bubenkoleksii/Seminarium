namespace CourseService.Application.LessonItem.Models;

public class LessonItemModelResponse
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public required string Title { get; set; }

    public string? Text { get; set; }

    public Guid LessonId { get; set; }

    public required LessonModelResponse Lesson { get; set; }

    public Guid? AuthorId { get; set; }

    public SchoolProfileContract? Author { get; set; }

    public ICollection<string>? Attachments { get; set; }

    public bool IsArchived { get; set; }
}
