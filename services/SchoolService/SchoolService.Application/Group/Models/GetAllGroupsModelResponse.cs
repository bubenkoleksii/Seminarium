namespace SchoolService.Application.Group.Models;

public record GetAllGroupsModelResponse(
    IEnumerable<GroupModelResponse> Entries,

    string SchoolName,

    ulong Total,

    uint Skip,

    uint Take
);
