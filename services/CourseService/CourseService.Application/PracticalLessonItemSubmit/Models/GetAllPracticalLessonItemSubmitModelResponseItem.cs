namespace CourseService.Application.PracticalLessonItemSubmit.Models;

public class GetAllPracticalLessonItemSubmitModelResponseItem
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid StudentId { get; set; }

    public string? StudentName { get; set; }

    public string? GroupName { get; set; }

    public Guid PracticalLessonItemId { get; set; }

    public ICollection<string>? Attachments { get; set; }

    public PracticalLessonItemSubmitStatus Status { get; set; }
}
