namespace SchoolService.Api.Models.SchoolProfile;

public record CreateSchoolProfileRequest(
    string InvitationCode,

    string? Phone,

    string? Details,

    string? TeachersExperience,

    string? TeachersEducation,

    string? TeachersQualification,

    uint? TeachersLessonsPerCycle,

    DateOnly? StudentsDateOfBirth,

    IEnumerable<Guid>? StudentsParentIds,

    string? StudentsAptitudes,

    string? ParentsAddress,

    string? ParentsRelationshipToChild
);
