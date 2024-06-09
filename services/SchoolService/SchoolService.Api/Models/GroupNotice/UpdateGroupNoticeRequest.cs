namespace SchoolService.Api.Models.GroupNotice;

public record UpdateGroupNoticeRequest(
    Guid Id,

    Guid GroupId,

    string Title,

    string? Text
);
