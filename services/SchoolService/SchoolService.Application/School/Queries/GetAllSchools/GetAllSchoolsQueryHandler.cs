namespace SchoolService.Application.School.Queries.GetAllSchools;

public class GetAllSchoolsQueryHandler : IRequestHandler<GetAllSchoolsQuery, GetAllSchoolsModelResponse>
{
    private const uint DefaultTake = 4;

    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    public GetAllSchoolsQueryHandler(IQueryContext queryContext, IMapper mapper)
    {
        _queryContext = queryContext;
        _mapper = mapper;
    }

    public async Task<GetAllSchoolsModelResponse> Handle(GetAllSchoolsQuery request, CancellationToken cancellationToken)
    {
        var dbQuery = _queryContext.Schools.AsQueryable();

        if (!string.IsNullOrEmpty(request.SchoolName))
            dbQuery = dbQuery.Where(r => r.Name.ToLower().Contains(request.SchoolName.ToLower()));

        if (request.SortByDateAsc.HasValue)
        {
            dbQuery = request.SortByDateAsc.Value
                ? dbQuery.OrderBy(r => r.CreatedAt)
                : dbQuery.OrderByDescending(r => r.CreatedAt);
        }

        if (request.Region.HasValue)
            dbQuery = dbQuery.Where(r => r.Region == request.Region);

        var take = request.Take ?? DefaultTake;
        var entities = await dbQuery
            .Skip((int)request.Skip)
            .Take((int)take)
            .ToListAsync(cancellationToken: cancellationToken);

        var schoolsResponse = _mapper.Map<IEnumerable<SchoolModelResponse>>(entities);

        var response = new GetAllSchoolsModelResponse(
            Entries: schoolsResponse,
            Total: (ulong)dbQuery.Count(),
            Skip: request.Skip,
            Take: take
        );

        return response;
    }
}
