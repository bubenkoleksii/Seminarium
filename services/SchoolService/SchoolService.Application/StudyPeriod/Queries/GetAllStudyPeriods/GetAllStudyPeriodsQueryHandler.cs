namespace SchoolService.Application.StudyPeriod.Queries.GetAllStudyPeriods;

public class GetAllStudyPeriodsQueryHandler(
    ISchoolProfileManager schoolProfileManager, IQueryContext queryContext, IMapper mapper)
    : IRequestHandler<GetAllStudyPeriodsQuery, IEnumerable<StudyPeriodModelResponse>>
{
    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly IQueryContext _queryContext = queryContext;

    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<StudyPeriodModelResponse>> Handle(GetAllStudyPeriodsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile == null)
            return [];

        var entities = await _queryContext.StudyPeriods
            .Include(period => period.School)
            .Where(period => period.SchoolId == profile.SchoolId)
            .OrderByDescending(period => period.StartDate)
            .ToListAsync();

        var studyPeriodsResponse = _mapper.Map<IEnumerable<StudyPeriodModelResponse>>(entities);
        return studyPeriodsResponse;
    }
}
