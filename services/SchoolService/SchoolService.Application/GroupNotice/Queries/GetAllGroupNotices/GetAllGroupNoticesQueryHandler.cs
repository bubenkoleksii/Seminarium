namespace SchoolService.Application.GroupNotice.Queries.GetAllGroupNotices;

public class GetAllGroupNoticesQueryHandler(ISchoolProfileManager schoolProfileManager, IQueryContext queryContext, IMapper mapper, IFilesManager filesManager)
    : IRequestHandler<GetAllGroupNoticesQuery, Either<GetAllGroupNoticesModelResponse, Error>>
{
    private const int DefaultTake = 8;

    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly IQueryContext _queryContext = queryContext;

    private readonly IMapper _mapper = mapper;

    private readonly IFilesManager _filesManager = filesManager;

    public async Task<Either<GetAllGroupNoticesModelResponse, Error>> Handle(GetAllGroupNoticesQuery request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile == null)
            return new InvalidError("school_profile");

        var group = await _queryContext.Groups
            .FindAsync(request.GroupId, CancellationToken.None);
        if (group is null)
            return new InvalidError("group_id");

        var validationResult = await ValidateProfile(profile.Type, profile.UserId, group);
        if (validationResult.IsSome)
            return (Error)validationResult;

        var lastNotice = await _queryContext.GroupNotices
            .Include(notice => notice.Author)
            .OrderByDescending(n => n.CreatedAt)
            .FirstOrDefaultAsync(n => n.GroupId == group.Id, cancellationToken: cancellationToken);
        if (lastNotice != null && lastNotice.Author != null)
        {
            lastNotice.Author.Notices = null;

            if (lastNotice.Author.Img is not null)
            {
                var image = _filesManager.GetFile(lastNotice.Author.Img);
                lastNotice.Author.Img = image.IsRight ? null : ((FileSuccess)image).Url;
            }
        }

        var lastNoticeResponse = _mapper.Map<GroupNoticeModelResponse?>(lastNotice);

        var dbQuery = _queryContext.GroupNotices
            .Include(notice => notice.Author)
            .Where(notice => notice.GroupId == group.Id)
            .AsQueryable();

        if (lastNoticeResponse != null)
            dbQuery = dbQuery.Where(n => n.Id != lastNoticeResponse.Id);

        if (request.MyOnly.HasValue && request.MyOnly.Value)
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
            .Include(notice => notice.Author)
            .OrderByDescending(n => n.IsCrucial)
            .Skip(request.Skip)
            .Take(take)
            .ToListAsync(cancellationToken: cancellationToken);

        entities = entities.Select(entity =>
        {
            if (entity.Author != null)
            {
                entity.Author.Notices = null;

                if (entity.Author.Img is not null)
                {
                    var image = _filesManager.GetFile(entity.Author.Img);
                    entity.Author.Img = image.IsRight ? null : ((FileSuccess)image).Url;
                }
            }

            return entity;
        }).ToList();

        var entitiesResponse = _mapper.Map<IEnumerable<GroupNoticeModelResponse>>(entities);

        var crucialNotes = entitiesResponse
            .Where(n => n.IsCrucial)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
        var regularNotes = entitiesResponse
            .Where(n => !n.IsCrucial)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();

        var count = (ulong)dbQuery.Count();
        if (lastNoticeResponse is not null)
            count++;

        var response = new GetAllGroupNoticesModelResponse(
            lastNoticeResponse,
            crucialNotes,
            regularNotes,
            Total: count,
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
