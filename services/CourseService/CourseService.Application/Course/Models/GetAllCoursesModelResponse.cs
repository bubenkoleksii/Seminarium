namespace CourseService.Application.Course.Models;

public record GetAllCoursesModelResponse(
    IEnumerable<CourseModelResponse> Entries,

    Guid? StudyPeriodId,

    ulong Total,

    uint Skip,

    uint Take
);
