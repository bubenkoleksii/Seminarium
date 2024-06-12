namespace Shared.Contracts.SchoolProfile.GetSchoolProfiles;

public class GetSchoolProfilesResponse : BaseResponse
{
    public IEnumerable<SchoolProfileContract>? Profiles { get; set; }
}
