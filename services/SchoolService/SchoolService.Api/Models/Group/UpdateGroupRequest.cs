namespace SchoolService.Api.Models.Group;

public record UpdateGroupRequest(
    Guid Id,

    string Name,

    byte StudyPeriodNumber
);
