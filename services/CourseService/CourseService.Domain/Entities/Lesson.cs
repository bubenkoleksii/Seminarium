namespace CourseService.Domain.Entities;

public class Lesson : Entity
{
    public required string Topic { get; set; }

    public string? Homework { get; set; }

    public Guid CourseId { get; set; }

    public required Course Course { get; set; }

    public IEnumerable<LessonItem>? LessonItems { get; set; }
}
