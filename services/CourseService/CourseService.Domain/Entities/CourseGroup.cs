namespace CourseService.Domain.Entities;

public class CourseGroup : Entity
{
    public IEnumerable<Course>? Courses { get; set; }
}
