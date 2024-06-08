namespace SchoolService.Api.Models.Group;

public record GetAllGroupsParams(
    string? Name,

    byte? StudyPeriodNumber,

    uint Skip,

    uint? Take
);
