namespace SchoolService.Application.SchoolProfile.Models;

public record GetAllSchoolProfilesBySchoolModelResponse(
    IEnumerable<SchoolProfileModelResponse> Entries,

    ulong Total,

    uint Skip,

    uint Take
);
