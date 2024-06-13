namespace CourseService.Application.LessonItem.Commands.PracticalLessonItem.CreatePracticalLessonItem;

public class CreatePracticalLessonItemCommand : IRequest<Either<PracticalLessonItemModelResponse, Error>>
{
    public Guid UserId { get; set; }

    public Guid LessonId { get; set; }

    public DateTime? Deadline { get; set; }

    public required string Title { get; set; }

    public string? Text { get; set; }

    public ICollection<AttachmentModelRequest>? Attachments { get; set; }

    public int? Attempts { get; set; }

    public bool AllowSubmitAfterDeadline { get; set; }
}
