using LanguageExt;

namespace Shared.Contracts.SchoolProfile.GetSchoolProfiles;

public class GetSchoolProfilesResponse
{
    public Either<IEnumerable<SchoolProfileContract>, Errors.Error> Result { get; set; }
}
