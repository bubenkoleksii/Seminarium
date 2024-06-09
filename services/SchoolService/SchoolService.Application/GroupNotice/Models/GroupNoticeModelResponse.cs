namespace SchoolService.Application.GroupNotice.Models;

public record GroupNoticeModelResponse(
    Guid Id,

    string Title,

    string? Text,

    bool IsCrucial,

    Guid GroupId,

    Guid? AuthorId,

    SchoolProfileModelResponse? Author
);
