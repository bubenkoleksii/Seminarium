namespace CourseService.Domain.Entities;

public class CourseGroupCourse
{
    public Guid Id { get; set; }

    public Guid? GroupId { get; set; }

    public CourseGroup? Group { get; set; }

    public Guid? CourseId { get; set; }

    public Course? Course { get; set; }
}
