namespace CourseService.Application.Course.Commands.CopyCourse;

public class CopyCourseCommand : IRequest<Either<CourseModelResponse, Error>>
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid StudyPeriodId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
