namespace SchoolService.Application.JoiningRequest.Models;

public record GetAllJoiningRequestsModelResponse(
    IEnumerable<JoiningRequestModelResponse> Entries,

    int Total,

    int Skip,

    int Take
);
