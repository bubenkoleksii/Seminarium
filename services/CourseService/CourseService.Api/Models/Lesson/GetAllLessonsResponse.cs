namespace CourseService.Api.Models.Lesson;

public record GetAllLessonsResponse(
    IEnumerable<LessonResponse> Entries,

    Guid? CourseId,

    ulong Total,

    uint Skip,

    uint Take
);
