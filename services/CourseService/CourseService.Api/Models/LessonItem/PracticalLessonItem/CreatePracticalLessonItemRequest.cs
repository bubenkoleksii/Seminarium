namespace CourseService.Api.Models.LessonItem.PracticalLessonItem;

public record CreatePracticalLessonItemRequest(
    Guid LessonId,

    DateTime? Deadline,

    string Title,

    string? Text,

    ICollection<IFormFile>? Attachments,

    int? Attempts,

    bool AllowSubmitAfterDeadline,

    bool IsArchived
);
