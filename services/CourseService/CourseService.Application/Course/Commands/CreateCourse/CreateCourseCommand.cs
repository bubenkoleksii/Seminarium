namespace CourseService.Application.Course.Commands.CreateCourse;

public class CreateCourseCommand : IRequest<Either<CourseModelResponse, Error>>
{
    public Guid UserId { get; set; }

    public Guid StudyPeriodId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
