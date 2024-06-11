namespace SchoolService.Api.Models.GroupNotice;

public record UpdateGroupNoticeRequest(
    Guid Id,

    Guid GroupId,

    bool IsCrucial,

    string Title,

    string? Text
);
