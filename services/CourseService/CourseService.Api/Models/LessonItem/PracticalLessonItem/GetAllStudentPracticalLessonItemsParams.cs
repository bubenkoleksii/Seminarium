namespace CourseService.Api.Models.LessonItem.PracticalLessonItem;

public record GetAllStudentPracticalLessonItemsParams(
    Guid StudentId,

    uint Skip,

    uint? Take
);
