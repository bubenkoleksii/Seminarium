namespace CourseService.Application.Lesson.Models;

public record GetAllLessonsModelResponse(
    IEnumerable<LessonModelResponse> Entries,

    Guid? CourseId,

    ulong Total,

    uint Skip,

    uint Take
);
