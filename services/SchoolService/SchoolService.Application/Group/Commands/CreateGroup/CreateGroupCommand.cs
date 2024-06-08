namespace SchoolService.Application.Group.Commands.CreateGroup;

public class CreateGroupCommand : IRequest<Either<GroupModelResponse, Error>>
{
    public required string Name { get; set; }

    public byte StudyPeriodNumber { get; set; }

    public Guid UserId { get; set; }
}
