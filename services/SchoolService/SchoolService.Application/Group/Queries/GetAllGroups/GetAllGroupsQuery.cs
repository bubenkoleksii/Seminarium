namespace SchoolService.Application.Group.Queries.GetAllGroups;

public class GetAllGroupsQuery : IRequest<Either<GetAllGroupsModelResponse, Error>>
{
    public Guid UserId { get; set; }

    public string? Name { get; init; }

    public byte? StudyPeriodNumber { get; init; }

    public uint Skip { get; init; }

    public uint? Take { get; init; }
}
