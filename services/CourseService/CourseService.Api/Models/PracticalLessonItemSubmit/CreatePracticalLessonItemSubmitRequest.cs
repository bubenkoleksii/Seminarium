namespace CourseService.Api.Models.PracticalLessonItemSubmit;

public record CreatePracticalLessonItemSubmitRequest(
    Guid PracticalLessonItemId,

    string? Text,

    ICollection<IFormFile>? Attachments
);
