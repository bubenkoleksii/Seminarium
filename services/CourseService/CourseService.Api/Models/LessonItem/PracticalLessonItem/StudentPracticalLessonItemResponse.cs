namespace CourseService.Api.Models.LessonItem.PracticalLessonItem;

public record StudentPracticalLessonItemResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? Deadline,

    string Title,

    string LessonTopic,

    string CourseName,

    string Status
);
