namespace SchoolService.Application.SchoolProfile.Models.Serialization;

internal record TeacherSerializationData(
    string? TeachersExperience,

    string? TeachersEducation,

    string? TeachersQualification,

    uint TeachersLessonsPerCycle
);
