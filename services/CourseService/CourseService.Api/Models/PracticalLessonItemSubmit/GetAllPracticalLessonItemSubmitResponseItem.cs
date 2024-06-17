namespace CourseService.Api.Models.PracticalLessonItemSubmit;

public record GetAllPracticalLessonItemSubmitResponseItem(
    Guid Id,

    DateTime CreatedAt,

    Guid StudentId,

    string? StudentName,

    string? GroupName,

    Guid PracticalLessonItemId,

    ICollection<string>? Attachments,

    string Status
);