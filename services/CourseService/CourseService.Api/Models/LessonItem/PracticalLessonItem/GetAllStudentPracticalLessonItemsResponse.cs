namespace CourseService.Api.Models.LessonItem.PracticalLessonItem;

public record GetAllStudentPracticalLessonItemsResponse(
    IEnumerable<StudentPracticalLessonItemResponse> Entries,

    Guid StudentId,

    ulong Total,

    uint Skip,

    uint Take
);
