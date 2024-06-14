namespace CourseService.Application.PracticalLessonItemSubmit.Commands.UpdatePracticalLessonItemSubmit;

public class UpdatePracticalLessonItemSubmitCommand : IRequest<Either<PracticalLessonItemSubmitModelResponse, Error>>
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string? Text { get; set; }
}
