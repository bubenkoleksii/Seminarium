namespace CourseService.Api.Models.LessonItem.TheoryLessonItem;

public record TheoryLessonItemResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string? Title,

    string? Text,

    Guid LessonId,

    LessonResponse Lesson,

    Guid? AuthorId,

    SchoolProfileContract? Author,

    ICollection<string>? Attachments,

    bool IsGraded,

    bool IsArchived
);
