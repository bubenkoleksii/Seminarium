namespace SchoolService.Application.GroupNotice.Commands.UpdateGroupNotice;

public class UpdateGroupNoticeCommand : IRequest<Either<GroupNoticeModelResponse, Error>>
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public required string Title { get; set; }

    public string? Text { get; set; }
}
