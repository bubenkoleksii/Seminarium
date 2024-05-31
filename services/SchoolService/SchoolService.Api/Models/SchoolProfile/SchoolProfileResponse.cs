namespace SchoolService.Api.Models.SchoolProfile;

public record SchoolProfileResponse(
    Guid Id,

    Guid UserId,

    Guid? SchoolId,

    string? SchoolName,

    Guid? GroupId,

    Guid? ClassTeacherGroupId,

    bool IsActive,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Type,

    string? Phone,

    string? Email,

    string? Img,

    string? Details,

    string? TeacherExperience,

    string? TeacherEducation,

    string? TeacherQualification,

    uint? TeacherLessonsPerCycle,

    DateOnly? StudentDateOfBirth,

    string? StudentAptitudes,

    bool? StudentIsClassLeader,

    bool? StudentIsIndividually,

    string? StudentHealthGroup,

    string? ParentAddress,

    string? ParentRelationship
);
