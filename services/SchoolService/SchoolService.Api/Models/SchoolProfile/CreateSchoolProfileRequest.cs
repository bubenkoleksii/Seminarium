﻿namespace SchoolService.Api.Models.SchoolProfile;

public record CreateSchoolProfileRequest(
    string InvitationCode,

    string Name,

    string? Phone,

    string? Email,

    string? Details,

    string? TeacherSubjects,

    string? TeacherExperience,

    string? TeacherEducation,

    string? TeacherQualification,

    uint? TeacherLessonsPerCycle,

    DateOnly? StudentDateOfBirth,

    string? StudentAptitudes,

    bool? StudentIsClassLeader,

    bool? StudentIsIndividually,

    string? StudentHealthGroup,

    string? ParentAddress
);
