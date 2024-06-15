namespace CourseService.Api.Models.Lesson;

public record UpdateLessonRequest(
    uint Number,

    Guid Id,

    string Topic,

    string? Homework
);
