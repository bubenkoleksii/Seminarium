namespace CourseService.Api.Models.Lesson;

public record CreateLessonRequest(
    Guid CourseId,

    string Topic,

    string? Homework
);
