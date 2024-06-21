namespace SchoolService.Application.SchoolProfile.Consumers;

public class GetSchoolProfilesConsumer(IMapper mapper, IQueryContext queryContext)
    : IConsumer<GetSchoolProfilesRequest>
{
    private readonly IMapper _mapper = mapper;

    private readonly IQueryContext _queryContext = queryContext;

    public async Task Consume(ConsumeContext<GetSchoolProfilesRequest> context)
    {
        var response = new GetSchoolProfilesResponse();

        var dbQuery = _queryContext.SchoolProfiles.AsQueryable();

        if (context.Message.SchoolId.HasValue)
            dbQuery = dbQuery.Where(profile => profile.SchoolId != null &&
            profile.SchoolId == context.Message.SchoolId.Value);

        if (context.Message.GroupId.HasValue)
            dbQuery = dbQuery.Where(profile => profile.GroupId != null &&
            profile.GroupId == context.Message.GroupId.Value);

        if (context.Message.UserId.HasValue)
            dbQuery = dbQuery.Where(profile => profile.UserId == context.Message.UserId.Value);

        if (context.Message.Ids != null && context.Message.Ids.Length > 0)
            dbQuery = dbQuery.Where(item => context.Message.Ids.Contains(item.Id));

        var entities = await dbQuery.ToListAsync();

        var profiles = _mapper.Map<IEnumerable<SchoolProfileContract>>(entities);
        response.Profiles = profiles;

        await context.RespondAsync(response);
    }
}
