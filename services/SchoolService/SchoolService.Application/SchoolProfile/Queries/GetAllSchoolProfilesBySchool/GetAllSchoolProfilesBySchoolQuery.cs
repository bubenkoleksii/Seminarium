namespace SchoolService.Application.SchoolProfile.Queries.GetAllSchoolProfilesBySchool;

public class GetAllSchoolProfilesBySchoolQuery : IRequest<GetAllSchoolProfilesBySchoolModelResponse>
{
    public Guid UserId { get; set; }

    public string? Name { get; set; }

    public SchoolProfileType? Type { get; set; }

    public string? Group { get; set; }

    public uint Skip { get; set; }

    public uint? Take { get; set; }
}
