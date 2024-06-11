namespace SchoolService.Api.Models.GroupNotice;

public record ChangeGroupNoticeCrucialRequest(
    Guid Id,

    bool IsCrucial
);
