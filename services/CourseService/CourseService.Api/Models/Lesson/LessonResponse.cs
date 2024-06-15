﻿namespace CourseService.Api.Models.Lesson;

public record LessonResponse(
    Guid Id,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Topic,

    string? Homework,

    Guid CourseId,

    CourseResponse Course
);
