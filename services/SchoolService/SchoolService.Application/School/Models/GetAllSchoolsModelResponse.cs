namespace SchoolService.Application.School.Models;

public record GetAllSchoolsModelResponse(
    IEnumerable<SchoolModelResponse> Entries,

    ulong Total,

    uint Skip,

    uint Take
);
