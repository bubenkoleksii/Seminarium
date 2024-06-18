namespace CourseService.Application.PracticalLessonItemSubmit.Models;

public record GetAllPracticalLessonItemSubmitModelResponse(
    IEnumerable<GetAllPracticalLessonItemSubmitModelResponseItem> Entries,

    ulong Total,

    uint Skip,

    uint Take
);
