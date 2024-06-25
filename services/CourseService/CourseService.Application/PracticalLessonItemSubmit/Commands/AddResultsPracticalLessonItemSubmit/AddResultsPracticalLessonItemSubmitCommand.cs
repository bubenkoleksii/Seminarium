namespace CourseService.Application.PracticalLessonItemSubmit.Commands.AddResultsPracticalLessonItemSubmit;

public class AddResultsPracticalLessonItemSubmitCommand : IRequest<Option<Error>>
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public bool IsAccept { get; set; }

    public string? Text { get; set; }

    public uint? Mark { get; set; }
}
