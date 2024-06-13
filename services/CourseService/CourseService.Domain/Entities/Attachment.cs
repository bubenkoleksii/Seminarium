namespace CourseService.Domain.Entities;

public class Attachment : Entity
{
    public required string Url { get; set; }

    public Guid? LessonItemId { get; set; }

    public LessonItem? LessonItem { get; set; }

    public Guid? PracticalLessonItemSubmitId { get; set; }

    public PracticalLessonItemSubmit? PracticalLessonItemSubmit { get; set; }
}
