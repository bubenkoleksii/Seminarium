namespace CourseService.Api.Models.Lesson;

public record LessonResponse(
    Guid Id,

    uint Number,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Topic,

    string? Homework,

    Guid CourseId,

    uint? PracticalItemsCount,

    uint? TheoryItemsCount
);
