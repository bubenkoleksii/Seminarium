namespace SchoolService.Application.Group.Queries.GetAllGroups;

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, Either<GetAllGroupsModelResponse, Error>>
{
    private const uint DefaultTake = 4;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    public GetAllGroupsQueryHandler(ISchoolProfileManager schoolProfileManager, IQueryContext queryContext, IMapper mapper)
    {
        _schoolProfileManager = schoolProfileManager;
        _queryContext = queryContext;
        _mapper = mapper;
    }

    public async Task<Either<GetAllGroupsModelResponse, Error>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null || profile.Type != SchoolProfileType.SchoolAdmin)
            return new InvalidError("school_profile");

        var school = await _queryContext.Schools.FindAsync(profile.SchoolId);
        if (school == null)
            return new InvalidError("school_id");

        var dbQuery = _queryContext.Groups
            .Where(g => g.SchoolId == school.Id)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Name))
            dbQuery = dbQuery.Where(g => g.Name.ToLower().Contains(request.Name.ToLower()));

        if (request.StudyPeriodNumber.HasValue)
            dbQuery = dbQuery.Where(g => g.StudyPeriodNumber == request.StudyPeriodNumber);

        dbQuery = dbQuery.OrderBy(g => g.StudyPeriodNumber);

        var take = request.Take ?? DefaultTake;
        var entities = await dbQuery
            .Skip((int)request.Skip)
            .Take((int)take)
            .ToListAsync(cancellationToken: cancellationToken);

        var groupsResponse = _mapper.Map<IEnumerable<GroupModelResponse>>(entities);

        var response = new GetAllGroupsModelResponse(
            Entries: groupsResponse,
            SchoolName: school.Name,
            Total: (ulong)dbQuery.Count(),
            Skip: request.Skip,
            Take: take
        );

        return response;
    }
}
