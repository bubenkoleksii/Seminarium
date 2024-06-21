namespace CourseService.Api.Models.LessonItem.TheoryLessonItem;

public record UpdateTheoryLessonItemRequest(
    Guid Id,

    Guid LessonId,

    DateTime? Deadline,

    string Title,

    string? Text,

    bool IsGraded
);
