﻿namespace SchoolService.Api.Models.SchoolProfile;

public class SchoolProfileResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public required string Name { get; set; }

    public Guid? SchoolId { get; set; }

    public SchoolResponse? School { get; set; }

    public string? SchoolName { get; set; }

    public Guid? GroupId { get; set; }

    public GroupResponse? Group { get; set; }

    public Guid? ClassTeacherGroupId { get; set; }

    public GroupResponse? ClassTeacherGroup { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public required string Type { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Img { get; set; }

    public string? Details { get; set; }

    public string? TeacherSubjects { get; set; }

    public string? TeacherExperience { get; set; }

    public string? TeacherEducation { get; set; }

    public string? TeacherQualification { get; set; }

    public uint? TeacherLessonsPerCycle { get; set; }

    public DateOnly? StudentDateOfBirth { get; set; }

    public string? StudentAptitudes { get; set; }

    public bool? StudentIsClassLeader { get; set; }

    public bool? StudentIsIndividually { get; set; }

    public string? StudentHealthGroup { get; set; }

    public string? ParentAddress { get; set; }

    public IEnumerable<SchoolProfileResponse>? Children { get; set; }

    public IEnumerable<SchoolProfileResponse>? Parents { get; set; }
}
