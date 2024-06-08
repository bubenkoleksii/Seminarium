namespace SchoolService.Application.SchoolProfile.Models.Serialization;

internal record TeacherSerializationData(
    string? TeachersSubjects,

    string? TeachersExperience,

    string? TeachersEducation,

    string? TeachersQualification,

    uint TeachersLessonsPerCycle
);
