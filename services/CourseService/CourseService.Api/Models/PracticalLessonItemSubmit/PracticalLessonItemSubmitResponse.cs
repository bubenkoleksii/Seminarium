namespace CourseService.Api.Models.PracticalLessonItemSubmit;

public record PracticalLessonItemSubmitResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    Guid StudentId,

    string? StudentName,

    Guid PracticalLessonItemId,

    int Attempt,

    string? Text,

    string? TeacherComment,

    uint? Mark,

    ICollection<string>? Attachments,

    string Status
);
