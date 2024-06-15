namespace CourseService.Api.Models.Course;

public record GetAllCoursesResponse(
    IEnumerable<CourseResponse> Entries,

    Guid? StudyPeriodId,

    ulong Total,

    uint Skip,

    uint Take
);
