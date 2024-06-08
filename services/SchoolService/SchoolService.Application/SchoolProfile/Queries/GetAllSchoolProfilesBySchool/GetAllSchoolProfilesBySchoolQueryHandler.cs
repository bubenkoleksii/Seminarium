namespace SchoolService.Application.SchoolProfile.Queries.GetAllSchoolProfilesBySchool;

public class GetAllSchoolProfilesBySchoolQueryHandler : IRequestHandler<GetAllSchoolProfilesBySchoolQuery, GetAllSchoolProfilesBySchoolModelResponse>
{
    private const uint DefaultTake = 4;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    public GetAllSchoolProfilesBySchoolQueryHandler(
        ISchoolProfileManager schoolProfileManager,
        IQueryContext queryContext,
        IMapper mapper)
    {
        _schoolProfileManager = schoolProfileManager;
        _queryContext = queryContext;
        _mapper = mapper;
    }

    public async Task<GetAllSchoolProfilesBySchoolModelResponse> Handle(GetAllSchoolProfilesBySchoolQuery request, CancellationToken cancellationToken)
    {
        var activeProfile = await _schoolProfileManager.GetActiveProfile(request.UserId);

        if (activeProfile is null || (activeProfile.Type != SchoolProfileType.SchoolAdmin &&
            activeProfile.Type != SchoolProfileType.ClassTeacher &&
            activeProfile.Type != SchoolProfileType.Teacher))
            return new GetAllSchoolProfilesBySchoolModelResponse(
                Entries: [],
                Total: 0,
                Skip: request.Skip,
                Take: DefaultTake
             );

        var dbQuery = _queryContext.SchoolProfiles
            .Include(profile => profile.Group)
            .Include(profile => profile.School)
            .Include(profile => profile.ClassTeacherGroup)
            .Where(profile => profile.SchoolId != null
                                ? profile.SchoolId == activeProfile.SchoolId
                                : profile.Group != null && profile.Group.SchoolId == activeProfile.SchoolId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Name))
            dbQuery = dbQuery.Where(p => p.Name.ToLower().Contains(request.Name.ToLower()));

        if (!string.IsNullOrEmpty(request.Group))
            dbQuery = dbQuery.Where(p => p.Group != null && p.Group.Name.ToLower().Contains(request.Group));

        if (request.Type.HasValue)
            dbQuery = dbQuery.Where(p => p.Type == request.Type);

        var take = request.Take ?? DefaultTake;
        var entities = await dbQuery
            .Skip((int)request.Skip)
            .Take((int)take)
            .ToListAsync(cancellationToken: cancellationToken);

        ClearCyclicDependencies(entities);

        var schoolsResponse = _mapper.Map<IEnumerable<SchoolProfileModelResponse>>(entities);

        var response = new GetAllSchoolProfilesBySchoolModelResponse(
            Entries: schoolsResponse,
            Total: (ulong)dbQuery.Count(),
            Skip: request.Skip,
            Take: take
        );

        return response;
    }

    private static void ClearCyclicDependencies(IEnumerable<Domain.Entities.SchoolProfile> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.Group != null)
            {
                entity.Group.ClassTeacher = null;
                entity.Group.Students = null;
            }

            if (entity.School != null)
            {
                entity.School.Groups = null;
                entity.School.Teachers = null;
            }

            if (entity.ClassTeacherGroup != null)
            {
                entity.ClassTeacherGroup.ClassTeacher = null;
                entity.ClassTeacherGroup.Students = null;
            }
        }
    }
}
