namespace SchoolService.Api.Models.SchoolProfile;

public record GetAllSchoolProfilesBySchoolResponse(
    IEnumerable<SchoolProfileResponse> Entries,

    ulong Total,

    uint Skip,

    uint Take
);
