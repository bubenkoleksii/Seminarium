using LanguageExt;

namespace Shared.Contracts.Group.GetGroups;

public class GetGroupsResponse
{
    public Either<IEnumerable<GroupContract>, Errors.Error> Result { get; set; }
}
