namespace CourseService.Domain.Entities;

public class PracticalLessonItem : LessonItem
{
    public int? Attempts { get; set; }

    public bool AllowSubmitAfterDeadline { get; set; }

    public IEnumerable<PracticalLessonItemSubmit>? Submits { get; set; }
}
