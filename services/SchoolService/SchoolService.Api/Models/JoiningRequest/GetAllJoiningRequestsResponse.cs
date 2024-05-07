namespace SchoolService.Api.Models.JoiningRequest;

public record GetAllJoiningRequestsResponse(
    IEnumerable<JoiningRequestResponse> Entries,

    int Total,

    int Skip,

    int Take
);
