namespace CourseService.Api.Models.Course;

public record AddCourseGroupRequest(
    string Name,

    Guid CourseId,

    Guid SchoolId
);
