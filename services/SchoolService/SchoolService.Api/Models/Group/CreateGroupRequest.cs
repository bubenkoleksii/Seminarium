namespace SchoolService.Api.Models.Group;

public record CreateGroupRequest(
    string Name,

    byte StudyPeriodNumber
);
