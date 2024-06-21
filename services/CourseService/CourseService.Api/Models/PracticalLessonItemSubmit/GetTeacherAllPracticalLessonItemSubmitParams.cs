namespace CourseService.Api.Models.PracticalLessonItemSubmit;

public record GetTeacherAllPracticalLessonItemSubmitParams(
    Guid ItemId,

    uint Skip,

    uint? Take
);
