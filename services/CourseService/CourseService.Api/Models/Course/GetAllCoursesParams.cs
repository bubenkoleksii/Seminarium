namespace CourseService.Api.Models.Course;

public record GetAllCoursesParams(
    Guid? StudyPeriodId,

    Guid? GroupId,

    Guid? TeacherId,

    string? Name,

    uint Skip,

    uint? Take
);
