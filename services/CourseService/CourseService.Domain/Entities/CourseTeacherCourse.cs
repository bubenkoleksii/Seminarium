namespace CourseService.Domain.Entities;

public class CourseTeacherCourse
{
    public Guid Id { get; set; }

    public Guid? TeacherId { get; set; }

    public CourseTeacher? Teacher { get; set; }

    public Guid? CourseId { get; set; }

    public Course? Course { get; set; }
}
