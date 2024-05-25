namespace SchoolService.Api.Models.School;

public record GetAllSchoolsResponse(
    IEnumerable<SchoolResponse> Entries,

    ulong Total,

    uint Skip,

    uint Take
);
