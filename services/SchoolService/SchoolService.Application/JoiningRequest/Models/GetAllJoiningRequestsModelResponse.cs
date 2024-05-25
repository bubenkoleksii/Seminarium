namespace SchoolService.Application.JoiningRequest.Models;

public record GetAllJoiningRequestsModelResponse(
    IEnumerable<JoiningRequestModelResponse> Entries,

    ulong Total,

    uint Skip,

    uint Take
);
