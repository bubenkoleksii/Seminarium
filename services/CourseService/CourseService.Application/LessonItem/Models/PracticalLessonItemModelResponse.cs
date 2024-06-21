namespace CourseService.Application.LessonItem.Models;

public class PracticalLessonItemModelResponse : LessonItemModelResponse
{
    public int? Attempts { get; set; }

    public bool AllowSubmitAfterDeadline { get; set; }
}
