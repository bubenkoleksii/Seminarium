﻿namespace CourseService.Domain.Entities;

public class CourseTeacher
{
    public Guid Id { get; set; }

    public IEnumerable<Course>? Courses { get; set; }
}
