namespace CourseService.Api.Models.Lesson;

public record CreateLessonRequest(
    Guid CourseId,

    string Topic,

    DateTime? Date,

    string? Homework
);
