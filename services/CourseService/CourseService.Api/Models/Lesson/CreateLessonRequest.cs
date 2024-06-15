namespace CourseService.Api.Models.Lesson;

public record CreateLessonRequest(
    Guid CourseId,

    uint Number,

    string Topic,

    string? Homework
);
