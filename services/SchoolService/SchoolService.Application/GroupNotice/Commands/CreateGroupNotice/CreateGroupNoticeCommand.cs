namespace SchoolService.Application.GroupNotice.Commands.CreateGroupNotice;

public class CreateGroupNoticeCommand : IRequest<Either<GroupNoticeModelResponse, Error>>
{
    public Guid UserId { get; set; }

    public Guid GroupId { get; set; }

    public bool IsCrucial { get; set; }

    public required string Title { get; set; }

    public string? Text { get; set; }
}
