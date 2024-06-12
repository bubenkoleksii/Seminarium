namespace Shared.Contracts.SchoolProfile.GetActiveSchoolProfile;

public class GetActiveSchoolProfileResponse : BaseResponse
{
    public SchoolProfileContract? SchoolProfile { get; set; }
}
