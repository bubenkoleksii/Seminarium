﻿namespace CourseService.Application.LessonItem.Models;

public class LessonItemModelResponse
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public string? Text { get; set; }

    public Guid? AuthorId { get; set; }

    public SchoolProfileContract? Author { get; set; }

    public ICollection<string>? Attachments { get; set; }
}
