namespace CourseService.Api.Models.PracticalLessonItemSubmit;

public record AddPracticalItemSubmitResultsRequest(
    Guid Id,

    bool IsAccept,

    string? Text,

    uint? Mark
);
