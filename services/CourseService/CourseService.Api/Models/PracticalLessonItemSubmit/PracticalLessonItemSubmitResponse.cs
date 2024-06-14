namespace CourseService.Api.Models.PracticalLessonItemSubmit;

public record PracticalLessonItemSubmitResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    Guid StudentId,

    Guid PracticalLessonItemId,

    PracticalLessonItemResponse PracticalLessonItem,

    uint Attempt,

    string? Text,

    ICollection<string>? Attachments,

    string Status
);
