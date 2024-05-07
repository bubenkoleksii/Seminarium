namespace SchoolService.Api.Models.JoiningRequest;

public record GetAllJoiningRequestsParams(
    string? SchoolName,

    bool? SortByDateAsc,

    string? Region
);
