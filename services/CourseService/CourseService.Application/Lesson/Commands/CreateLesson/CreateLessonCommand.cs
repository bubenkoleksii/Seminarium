namespace CourseService.Application.Lesson.Commands.CreateLesson;

public class CreateLessonCommand : IRequest<Either<LessonModelResponse, Error>>
{
    public Guid UserId { get; set; }

    public Guid CourseId { get; set; }

    public required string Topic { get; set; }

    public string? Homework { get; set; }
}
