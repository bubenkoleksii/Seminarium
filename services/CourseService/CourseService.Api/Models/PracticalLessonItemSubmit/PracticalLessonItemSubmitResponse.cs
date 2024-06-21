namespace CourseService.Api.Models.PracticalLessonItemSubmit;

public record PracticalLessonItemSubmitResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    Guid StudentId,

    string? StudentName,

    Guid PracticalLessonItemId,

    string? Text,

    ICollection<string>? Attachments,

    string Status
);
