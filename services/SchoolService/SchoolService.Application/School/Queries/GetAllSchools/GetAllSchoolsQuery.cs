namespace SchoolService.Application.School.Queries.GetAllSchools;

public record GetAllSchoolsQuery(
    string? SchoolName,

    bool? SortByDateAsc,

    SchoolRegion? Region,

    uint Skip,

    uint? Take
) : IRequest<GetAllSchoolsModelResponse>;
