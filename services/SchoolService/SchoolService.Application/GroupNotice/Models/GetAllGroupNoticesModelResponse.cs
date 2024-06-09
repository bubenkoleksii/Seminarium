namespace SchoolService.Application.GroupNotice.Models;

public record GetAllGroupNoticesModelResponse(
    GroupNoticeModelResponse? LastNotice,

    IEnumerable<GroupNoticeModelResponse> CrucialNotices,

    IEnumerable<GroupNoticeModelResponse> RegularNotices,

    ulong Total,

    int Skip,

    int Take
);
