namespace SchoolService.Application.SchoolProfile.Models;

internal record TeacherSerializationData(
    string? TeachersExperience,

    string? TeachersEducation,

    string? TeachersQualification,

    uint TeachersLessonsPerCycle
);
