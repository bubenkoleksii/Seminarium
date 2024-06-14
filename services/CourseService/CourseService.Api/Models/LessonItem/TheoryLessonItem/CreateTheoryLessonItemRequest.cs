namespace CourseService.Api.Models.LessonItem.TheoryLessonItem;

public record CreateTheoryLessonItemRequest(
    Guid LessonId,

    DateTime? Deadline,

    string Title,

    string? Text,

    ICollection<IFormFile>? Attachments,

    bool IsGraded,

    bool IsArchived
);
