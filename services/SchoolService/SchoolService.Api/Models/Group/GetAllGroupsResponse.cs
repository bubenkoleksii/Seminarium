namespace SchoolService.Api.Models.Group;

public record GetAllGroupsResponse(
    IEnumerable<GroupResponse> Entries,

    string SchoolName,

    ulong Total,

    uint Skip,

    uint Take
);
