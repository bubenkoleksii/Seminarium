namespace CourseService.Domain.Entities;

public class CourseTeacher : Entity
{
    public IEnumerable<Course>? Courses { get; set; }
}
