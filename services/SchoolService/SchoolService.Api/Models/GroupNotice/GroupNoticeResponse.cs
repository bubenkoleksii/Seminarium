namespace SchoolService.Api.Models.GroupNotice;

public record GroupNoticeResponse(
    Guid Id,

    string Title,

    string? Text,

    bool IsCrucial,

    Guid GroupId,

    Guid? AuthorId,

    DateTime CreatedAt,

    DateTime? LastUpdatedAt,

    SchoolProfileResponse? Author
);
