namespace SchoolService.Application.GroupNotice.Queries.GetAllGroupNotices;

public class GetAllGroupNoticesQueryHandler(ISchoolProfileManager schoolProfileManager, IQueryContext queryContext, IMapper mapper)
    : IRequestHandler<GetAllGroupNoticesQuery, Either<GetAllGroupNoticesModelResponse, Error>>
{
    private const int DefaultTake = 15;

    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly IQueryContext _queryContext = queryContext;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<GetAllGroupNoticesModelResponse, Error>> Handle(GetAllGroupNoticesQuery request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile == null)
            return new InvalidError("school_profile");

        var group = await _queryContext.Groups.FindAsync(request.GroupId, CancellationToken.None);
        if (group is null)
            return new InvalidError("group_id");

        var validationResult = await ValidateProfile(profile.Type, profile.UserId, group);
        if (validationResult.IsSome)
            return (Error)validationResult;

        var lastNotice = await _queryContext.GroupNotices
            .OrderByDescending(n => n.CreatedAt)
            .FirstOrDefaultAsync(n => n.GroupId == group.Id, cancellationToken: cancellationToken);

        var lastNoticeResponse = _mapper.Map<GroupNoticeModelResponse?>(lastNotice);

        var dbQuery = _queryContext.GroupNotices
            .Include(notice => notice.Author)
            .Where(notice => notice.GroupId == group.Id)
            .AsQueryable();

        if (lastNoticeResponse != null)
            dbQuery = dbQuery.Where(n => n.Id != lastNoticeResponse.Id);

        if (request.MyOnly)
            dbQuery = dbQuery.Where(notice => notice.AuthorId == profile.Id);

        if (!string.IsNullOrEmpty(request.Search))
        {
            dbQuery = dbQuery.Where(notice =>
                EF.Functions.ToTsVector("simple", notice.Title + " " + notice.Text)
                .Matches(request.Search)
            );
        }

        var take = request.Take ?? DefaultTake;

        var entities = await dbQuery
            .OrderBy(n => n.IsCrucial)
            .ThenByDescending(n => n.CreatedAt)
            .Skip(request.Skip)
            .Take(take)
            .ToListAsync(cancellationToken: cancellationToken);

        var entitiesResponse = _mapper.Map<IEnumerable<GroupNoticeModelResponse>>(entities);

        var crucialNotes = entitiesResponse.Where(n => n.IsCrucial).ToList();
        var regularNotes = entitiesResponse.Where(n => !n.IsCrucial).ToList();

        var response = new GetAllGroupNoticesModelResponse(
            lastNoticeResponse,
            crucialNotes,
            regularNotes,
            Total: (ulong)dbQuery.Count(),
            Skip: request.Skip,
            Take: take
        );

        return response;
    }

    private async Task<Option<Error>> ValidateProfile(SchoolProfileType type, Guid userId, Domain.Entities.Group group)
    {
        switch (type)
        {
            case SchoolProfileType.SchoolAdmin or SchoolProfileType.Teacher:
                {
                    var validationError =
                        await _schoolProfileManager.ValidateSchoolProfileBySchool(userId, group.SchoolId);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.Student:
                {
                    var validationError = await _schoolProfileManager.ValidateSchoolProfileByGroup(userId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.ClassTeacher:
                {
                    var validationError = await _schoolProfileManager.ValidateClassTeacherSchoolProfileByGroup(userId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.Parent:
                {
                    var validationError = await _schoolProfileManager.ValidateParentProfileByChildGroup(userId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
        }

        return Option<Error>.None;
    }
}
