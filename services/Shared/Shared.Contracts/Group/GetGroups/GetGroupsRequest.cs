namespace Shared.Contracts.Group.GetGroups;

public record GetGroupsRequest(
    Guid[]? Ids,

    Guid? SchoolId
);
