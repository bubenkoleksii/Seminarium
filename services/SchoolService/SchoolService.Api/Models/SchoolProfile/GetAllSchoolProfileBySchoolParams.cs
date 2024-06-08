namespace SchoolService.Api.Models.SchoolProfile;

public record GetAllSchoolProfileBySchoolParams(
    string? Name,

    string? Type,

    string? Group,

    uint Skip,

    uint? Take
);
