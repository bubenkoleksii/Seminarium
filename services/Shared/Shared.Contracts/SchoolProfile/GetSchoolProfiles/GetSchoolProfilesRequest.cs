namespace Shared.Contracts.SchoolProfile.GetSchoolProfiles;

public record GetSchoolProfilesRequest(
    Guid? UserId,

    Guid? GroupId,

    Guid? SchoolId,

    string? Type
);
