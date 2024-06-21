namespace Shared.Contracts.SchoolProfile.GetSchoolProfiles;

public record GetSchoolProfilesRequest(
    Guid[]? Ids,

    Guid? UserId,

    Guid? GroupId,

    Guid? SchoolId,

    string? Type
);
