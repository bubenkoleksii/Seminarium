namespace SchoolService.Application.Group.Commands.UpdateGroup;

public class UpdateGroupCommand : IRequest<Either<GroupModelResponse, Error>>
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public byte StudyPeriodNumber { get; set; }

    public Guid UserId { get; set; }
}
