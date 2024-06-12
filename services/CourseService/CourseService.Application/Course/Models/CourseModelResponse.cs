namespace CourseService.Application.Course.Models;

public class CourseModelResponse
{
    public Guid Id { get; set; }

    public Guid StudyPeriodId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
