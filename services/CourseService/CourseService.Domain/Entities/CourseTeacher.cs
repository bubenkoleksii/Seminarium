namespace CourseService.Domain.Entities;

public class CourseTeacher
{
    public Guid Id { get; set; }

    public bool IsCreator { get; set; }

    public ICollection<Course>? Courses { get; set; }

    public IEnumerable<LessonItem>? LessonItems { get; set; }
}
