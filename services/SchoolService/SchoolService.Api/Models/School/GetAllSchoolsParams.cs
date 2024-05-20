namespace SchoolService.Api.Models.School;

public record GetAllSchoolsParams(
    string? SchoolName,

    bool? SortByDateAsc,

    string? Region,

    uint Skip,

    uint? Take
);
