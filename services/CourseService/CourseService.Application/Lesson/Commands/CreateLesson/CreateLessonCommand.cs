namespace CourseService.Application.Lesson.Commands.CreateLesson;

public class CreateLessonCommand : IRequest<Either<LessonModelResponse, Error>>
{
    public uint Number { get; set; }

    public Guid UserId { get; set; }

    public Guid CourseId { get; set; }

    public required string Topic { get; set; }

    public string? Homework { get; set; }
}
