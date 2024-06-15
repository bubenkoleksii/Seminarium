namespace CourseService.Application.Lesson.Commands.UpdateLesson;

public class UpdateLessonCommand : IRequest<Either<LessonModelResponse, Error>>
{
    public uint Number { get; set; }

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public required string Topic { get; set; }

    public DateTime? Date { get; set; }

    public string? Homework { get; set; }
}
