namespace CourseService.Api.Models.Lesson;

public record UpdateLessonRequest(
    Guid Id,

    string Topic,

    string? Homework
);
