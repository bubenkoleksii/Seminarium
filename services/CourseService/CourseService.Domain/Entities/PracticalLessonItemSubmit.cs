namespace CourseService.Domain.Entities;

public class PracticalLessonItemSubmit : Entity
{
    public Guid StudentId { get; set; }

    public Guid PracticalLessonItemId { get; set; }

    public required PracticalLessonItem PracticalLessonItem { get; set; }

    public uint Attempt { get; set; }

    public string? Text { get; set; }

    public ICollection<Attachment>? Attachments { get; set; }

    public PracticalLessonItemSubmitStatus Status { get; set; }

    public string? TeacherComment { get; set; }
}
