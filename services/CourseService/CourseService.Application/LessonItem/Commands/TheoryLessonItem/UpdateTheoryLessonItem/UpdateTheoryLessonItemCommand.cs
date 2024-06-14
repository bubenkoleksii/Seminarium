namespace CourseService.Application.LessonItem.Commands.TheoryLessonItem.UpdateTheoryLessonItem;

public class UpdateTheoryLessonItemCommand
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid LessonId { get; set; }

    public DateTime? Deadline { get; set; }

    public required string Title { get; set; }

    public string? Text { get; set; }

    public bool IsGraded { get; set; }
}
