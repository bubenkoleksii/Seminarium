namespace SchoolService.Application.SchoolProfile.Queries.GetUserSchoolProfiles;

public record GetUserSchoolProfilesQuery(
    Guid UserId
) : IRequest<IEnumerable<SchoolProfileModelResponse>>;
