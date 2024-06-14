namespace CourseService.Application.LessonItem.Commands.TheoryLessonItem.CreateTheoryLessonItem;

public class CreateTheoryLessonItemCommand : IRequest<Either<TheoryLessonItemModelResponse, Error>>
{
    public Guid UserId { get; set; }

    public Guid LessonId { get; set; }

    public DateTime? Deadline { get; set; }

    public required string Title { get; set; }

    public string? Text { get; set; }

    public ICollection<AttachmentModelRequest>? Attachments { get; set; }

    public bool IsGraded { get; set; }

    public bool IsArchived { get; set; }
}
