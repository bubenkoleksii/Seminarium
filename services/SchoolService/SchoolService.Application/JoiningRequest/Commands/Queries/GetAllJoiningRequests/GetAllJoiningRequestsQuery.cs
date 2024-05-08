﻿namespace SchoolService.Application.JoiningRequest.Commands.Queries.GetAllJoiningRequests;

public record GetAllJoiningRequestsQuery(
    string? SchoolName,

    bool? SortByDateAsc,

    SchoolRegion? Region,

    uint Skip,

    uint? Take
) : IRequest<GetAllJoiningRequestsModelResponse>;
