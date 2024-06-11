namespace SchoolService.Api.Models.GroupNotice;

public record CreateGroupNoticeRequest(
    Guid GroupId,

    bool IsCrucial,

    string Title,

    string? Text
);
