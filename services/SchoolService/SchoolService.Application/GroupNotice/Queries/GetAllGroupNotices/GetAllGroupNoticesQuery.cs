namespace SchoolService.Application.GroupNotice.Queries.GetAllGroupNotices;

public class GetAllGroupNoticesQuery : IRequest<Either<GetAllGroupNoticesModelResponse, Error>>
{
    public Guid UserId { get; set; }

    public Guid GroupId { get; set; }

    public bool MyOnly { get; set; }

    public string? Search { get; set; }

    public int Skip { get; set; }

    public int? Take { get; set; }
}
