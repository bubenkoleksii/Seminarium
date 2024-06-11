namespace SchoolService.Application.GroupNotice.Commands.ChangeGroupNoticeCrucial;

public record ChangeGroupNoticeCrucialCommand(
    Guid Id,

    Guid UserId,

    bool IsCrucial
) : IRequest<Either<GroupNoticeModelResponse, Error>>;
