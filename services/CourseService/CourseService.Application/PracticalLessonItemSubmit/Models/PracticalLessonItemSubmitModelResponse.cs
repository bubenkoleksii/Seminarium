namespace CourseService.Application.PracticalLessonItemSubmit.Models;

public class PracticalLessonItemSubmitModelResponse
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public Guid StudentId { get; set; }

    public Guid PracticalLessonItemId { get; set; }

    public required PracticalLessonItem PracticalLessonItem { get; set; }

    public uint Attempt { get; set; }

    public string? Text { get; set; }

    public ICollection<string>? Attachments { get; set; }

    public PracticalLessonItemSubmitStatus Status { get; set; }
}
