namespace CourseService.Api.Models.LessonItem.PracticalLessonItem;

public record UpdatePracticalLessonItemRequest(
    Guid Id,

    DateTime? Deadline,

    string Title,

    string? Text,

    int? Attempts,

    bool AllowSubmitAfterDeadline
);
