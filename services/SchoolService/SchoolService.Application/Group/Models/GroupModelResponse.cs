namespace SchoolService.Application.Group.Models;

public class GroupModelResponse
{
    public Guid Id { get; init; }

    public Guid SchoolId { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? LastUpdatedAt { get; init; }

    public required string Name { get; init; }

    public byte StudyPeriodNumber { get; init; }

    public string? Img { get; set; }
}
