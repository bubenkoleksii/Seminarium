namespace CourseService.Api.Models.Course;

public record AddCourseTeacherRequest(
    string Name,

    Guid CourseId,

    Guid SchoolId
);