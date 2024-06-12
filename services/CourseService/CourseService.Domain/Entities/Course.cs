namespace CourseService.Domain.Entities;

public class Course : Entity
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public IEnumerable<CourseTeacher>? Teachers { get; set; }

    public IEnumerable<CourseGroup>? Groups { get; set; }
}
