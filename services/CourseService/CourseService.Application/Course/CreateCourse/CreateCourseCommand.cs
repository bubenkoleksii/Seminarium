namespace CourseService.Application.Course.CreateCourse;

public class CreateCourseCommand : IRequest<int>
{
    public Guid UserId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
