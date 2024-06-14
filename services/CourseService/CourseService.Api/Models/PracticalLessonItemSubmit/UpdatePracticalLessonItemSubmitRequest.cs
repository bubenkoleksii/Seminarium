namespace CourseService.Api.Models.PracticalLessonItemSubmit;

public record UpdatePracticalLessonItemSubmitRequest(
    Guid Id,

    string? Text
);
