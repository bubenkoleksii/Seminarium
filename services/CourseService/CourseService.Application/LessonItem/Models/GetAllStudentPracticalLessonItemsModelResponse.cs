namespace CourseService.Application.LessonItem.Models;

public record GetAllStudentPracticalLessonItemsModelResponse(
    IEnumerable<StudentPracticalLessonItemModelResponse> Entries,

    Guid StudentId,

    ulong Total,

    uint Skip,

    uint Take
);
