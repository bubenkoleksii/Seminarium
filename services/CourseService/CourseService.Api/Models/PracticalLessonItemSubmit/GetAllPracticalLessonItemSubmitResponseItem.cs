namespace CourseService.Api.Models.PracticalLessonItemSubmit;

public record GetAllPracticalLessonItemSubmitResponseItem(
    Guid Id,

    DateTime CreatedAt,

    Guid StudentId,

    string? StudentName,

    string? GroupName,

    uint? Mark,

    Guid PracticalLessonItemId,

    string Status
);