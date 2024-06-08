namespace SchoolService.Domain.Entities;

public class SchoolProfile : Entity
{
    public Guid UserId { get; set; }

    public required string Name { get; set; }

    public SchoolProfileType Type { get; set; }

    public bool IsActive { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Img { get; set; }

    public string? Details { get; set; }

    public string? Data { get; set; }

    public Guid? SchoolId { get; set; }

    public School? School { get; set; }

    public Guid? GroupId { get; set; }

    public Group? Group { get; set; }

    public Guid? ClassTeacherGroupId { get; set; }

    public Group? ClassTeacherGroup { get; set; }

    public ICollection<SchoolProfile>? Parents { get; set; } = [];

    public ICollection<SchoolProfile>? Children { get; set; } = [];
}
