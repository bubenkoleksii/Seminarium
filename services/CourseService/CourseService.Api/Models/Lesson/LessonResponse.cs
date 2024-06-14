namespace CourseService.Api.Models.Lesson;

public record LessonResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Topic,

    DateTime? Date,

    string? Homework,

    Guid CourseId,

    CourseResponse Course
);
