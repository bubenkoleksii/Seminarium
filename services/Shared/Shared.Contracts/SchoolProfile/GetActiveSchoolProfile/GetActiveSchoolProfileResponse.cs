using LanguageExt;

namespace Shared.Contracts.SchoolProfile.GetActiveSchoolProfile;

public class GetActiveSchoolProfileResponse
{
    public Either<SchoolProfileContract, Errors.Error> Result { get; set; }
}
