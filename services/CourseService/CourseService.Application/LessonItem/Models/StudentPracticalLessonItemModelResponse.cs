namespace CourseService.Application.LessonItem.Models;

public class StudentPracticalLessonItemModelResponse
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? Deadline { get; set; }

    public string? Title { get; set; }

    public string? LessonTopic { get; set; }

    public string? CourseName { get; set; }

    public PracticalLessonItemSubmitStatus Status { get; set; }
}
