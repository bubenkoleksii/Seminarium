namespace CourseService.Domain.Entities;

public class CourseGroup
{
    public Guid Id { get; set; }

    public IEnumerable<Course>? Courses { get; set; }
}
