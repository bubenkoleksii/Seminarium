namespace CourseService.Api.Models.PracticalLessonItemSubmit;

public record GetAllPracticalLessonItemSubmitResponse(
    IEnumerable<GetAllPracticalLessonItemSubmitResponseItem> Entries,

    ulong Total,

    uint Skip,

    uint Take
);
