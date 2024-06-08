namespace SchoolService.Api.Models.Group;

public record OneGroupResponse(
    Guid Id,
    
    Guid SchoolId,

    string SchoolName,

    Guid? ClassTeacherId,

    SchoolProfileResponse? ClassTeacher,

    IEnumerable<SchoolProfileResponse>? Students,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    string Name,

    byte StudyPeriodNumber,

    string? Img
);
