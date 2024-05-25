namespace SchoolService.Api.Models.JoiningRequest;

public record GetAllJoiningRequestsResponse(
    IEnumerable<JoiningRequestResponse> Entries,

    ulong Total,

    uint Skip,

    uint Take
);
