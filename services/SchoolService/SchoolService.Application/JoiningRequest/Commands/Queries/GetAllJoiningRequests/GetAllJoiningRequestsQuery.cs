using SchoolService.Domain.Enums.JoiningRequest;

namespace SchoolService.Application.JoiningRequest.Commands.Queries.GetAllJoiningRequests;

public record GetAllJoiningRequestsQuery(
    string? SchoolName,

    bool? SortByDateAsc,

    SchoolRegion? Region,

    JoiningRequestStatus? Status,

    uint Skip,

    uint? Take
) : IRequest<GetAllJoiningRequestsModelResponse>;
