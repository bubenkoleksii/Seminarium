namespace CourseService.Domain.Entities;

public class Course : Entity
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public Guid StudyPeriodId { get; set; }

    public ICollection<CourseTeacher>? Teachers { get; set; }

    public ICollection<CourseGroup>? Groups { get; set; }

    public ICollection<Lesson>? Lessons { get; set; }
}
