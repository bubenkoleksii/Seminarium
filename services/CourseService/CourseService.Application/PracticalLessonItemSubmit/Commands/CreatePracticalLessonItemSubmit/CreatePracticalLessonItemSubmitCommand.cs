namespace CourseService.Application.PracticalLessonItemSubmit.Commands.CreatePracticalLessonItemSubmit;

public class CreatePracticalLessonItemSubmitCommand : IRequest<Either<PracticalLessonItemSubmitModelResponse, Error>>
{
    public Guid UserId { get; set; }

    public Guid PracticalLessonItemId { get; set; }

    public string? Text { get; set; }

    public ICollection<AttachmentModelRequest>? Attachments { get; set; }
}
