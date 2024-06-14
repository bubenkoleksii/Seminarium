namespace CourseService.Application.LessonItem.Commands.PracticalLessonItem.UpdatePracticalLessonItem;

public class UpdatePracticalLessonItemCommand
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid LessonId { get; set; }

    public DateTime? Deadline { get; set; }

    public required string Title { get; set; }

    public string? Text { get; set; }

    public int? Attempts { get; set; }

    public bool AllowSubmitAfterDeadline { get; set; }
}
}
