namespace SchoolService.Api.Models.GroupNotice;

public record GetAllGroupNoticesParams(
    Guid GroupId,

    bool MyOnly,

    string Search,

    uint Skip,

    uint? Take
);
