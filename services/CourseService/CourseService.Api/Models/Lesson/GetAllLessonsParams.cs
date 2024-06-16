namespace CourseService.Api.Models.Lesson;

public record GetAllLessonsParams(
    Guid CourseId,

    string? Topic
);
