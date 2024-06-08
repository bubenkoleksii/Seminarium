namespace SchoolService.Application.Group.Models;

public class OneGroupModelResponse
{
    public Guid Id { get; set; }

    public Guid SchoolId { get; set; }

    public required string SchoolName { get; set; }

    public Guid? ClassTeacherId { get; set; }

    public SchoolProfileModelResponse? ClassTeacher { get; set; }

    public IEnumerable<SchoolProfileModelResponse>? Students { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public required string Name { get; set; }

    public byte StudyPeriodNumber { get; set; }

    public string? Img { get; set; }
}
