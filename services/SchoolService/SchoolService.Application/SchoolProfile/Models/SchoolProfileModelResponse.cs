namespace SchoolService.Application.SchoolProfile.Models;

public class SchoolProfileModelResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid? SchoolId { get; set; }

    public Guid? GroupId { get; set; }

    public Guid? ClassTeacherGroupId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public SchoolProfileType Type { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Img { get; set; }

    public string? Details { get; set; }
}
