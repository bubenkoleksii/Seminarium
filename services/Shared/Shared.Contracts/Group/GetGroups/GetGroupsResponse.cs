namespace Shared.Contracts.Group.GetGroups;

public class GetGroupsResponse : BaseResponse
{
    public IEnumerable<GroupContract>? Groups { get; set; }
}
