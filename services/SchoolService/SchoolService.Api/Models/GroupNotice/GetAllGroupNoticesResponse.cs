namespace SchoolService.Api.Models.GroupNotice;

public record GetAllGroupNoticesResponse(
    GroupNoticeResponse? LastNotice,

    IEnumerable<GroupNoticeResponse> CrucialNotices,

    IEnumerable<GroupNoticeResponse> RegularNotices,

    ulong Total,

    uint Skip,

    uint Take
);
