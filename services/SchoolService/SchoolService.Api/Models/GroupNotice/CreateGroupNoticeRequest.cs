namespace SchoolService.Api.Models.GroupNotice;

public record CreateGroupNoticeRequest(
    Guid GroupId,

    string Title,

    string? Text
);
