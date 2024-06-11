namespace SchoolService.Domain.Entities;

public class Group : Entity
{
    public required string Name { get; set; }

    public byte StudyPeriodNumber { get; set; }

    public string? Img { get; set; }

    public Guid SchoolId { get; set; }

    public required School School { get; set; }

    public Guid? ClassTeacherId { get; set; }

    public SchoolProfile? ClassTeacher { get; set; }

    public IEnumerable<SchoolProfile>? Students { get; set; }

    public IEnumerable<GroupNotice>? Notices { get; set; }
}
