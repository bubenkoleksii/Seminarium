namespace Shared.Contracts.SchoolProfile.GetActiveSchoolProfile;

public record GetActiveSchoolProfileRequest(
    Guid UserId,

    string[] AllowedProfileTypes
);
