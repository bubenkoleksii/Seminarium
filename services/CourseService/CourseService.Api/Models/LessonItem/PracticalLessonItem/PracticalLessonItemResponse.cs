namespace CourseService.Api.Models.LessonItems.PracticalLessonItems;

public record PracticalLessonItemResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string? Title,

    string? Text,

    Guid LessonId,

    Guid? AuthorId,

    SchoolProfileContract? Author,

    ICollection<string>? Attachments,

    int? Attempts,

    bool AllowSubmitAfterDeadline,

    bool IsArchived
);
