namespace CourseService.Application.PracticalLessonItemSubmit.Commands.UpdatePracticalLessonItemSubmit;

public class UpdatePracticalLessonItemSubmitCommand
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid PracticalLessonItemId { get; set; }

    public string? Text { get; set; }
}
