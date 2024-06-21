namespace CourseService.Application.Lesson.Models;

public class LessonModelResponse
{
    public Guid Id { get; set; }

    public uint Number { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public required string Topic { get; set; }

    public string? Homework { get; set; }

    public Guid CourseId { get; set; }

    public uint? PracticalItemsCount { get; set; }

    public uint? TheoryItemsCount { get; set; }
}
