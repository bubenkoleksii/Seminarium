namespace SchoolService.Application.Group.Queries.GetAllGroups;

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, Either<GetAllGroupsModelResponse, Error>>
{
    private const uint DefaultTake = 4;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    private readonly IFilesManager _filesManager;

    public GetAllGroupsQueryHandler(
        ISchoolProfileManager schoolProfileManager,
        IQueryContext queryContext,
        IMapper mapper,
        IFilesManager filesManager)
    {
        _schoolProfileManager = schoolProfileManager;
        _queryContext = queryContext;
        _mapper = mapper;
        _filesManager = filesManager;
    }

    public async Task<Either<GetAllGroupsModelResponse, Error>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null || profile.Type != SchoolProfileType.SchoolAdmin && profile.Type != SchoolProfileType.Teacher &&
            profile.Type != SchoolProfileType.ClassTeacher)
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

        var groupsResponse = _mapper.Map<ICollection<GroupModelResponse>>(entities)
            .Select(item =>
            {
                if (item.Img is not null)
                {
                    var image = _filesManager.GetFile(item.Img);
                    item.Img = image.IsRight ? null : ((FileSuccess)image).Url;
                }

                return item;
            })
            .ToList();

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
