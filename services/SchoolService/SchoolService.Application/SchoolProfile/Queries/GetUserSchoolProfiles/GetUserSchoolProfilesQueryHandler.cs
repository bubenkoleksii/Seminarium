namespace SchoolService.Application.SchoolProfile.Queries.GetUserSchoolProfiles;

public class GetUserSchoolProfilesQueryHandler : IRequestHandler<GetUserSchoolProfilesQuery, IEnumerable<SchoolProfileModelResponse>>
{
    private readonly ISchoolProfileManager _schoolProfileManager;

    public GetUserSchoolProfilesQueryHandler(ISchoolProfileManager schoolProfileManager)
    {
        _schoolProfileManager = schoolProfileManager;
    }

    public async Task<IEnumerable<SchoolProfileModelResponse>> Handle(GetUserSchoolProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = await _schoolProfileManager.GetProfiles(request.UserId);
        return profiles ?? Enumerable.Empty<SchoolProfileModelResponse>();
    }
}
