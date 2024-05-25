namespace SchoolService.Application.JoiningRequest.Queries.GetAllJoiningRequests;

public record GetAllJoiningRequestsQuery(
    string? SchoolName,

    bool? SortByDateAsc,

    SchoolRegion? Region,

    JoiningRequestStatus? Status,

    uint Skip,

    uint? Take
) : IRequest<GetAllJoiningRequestsModelResponse>;
