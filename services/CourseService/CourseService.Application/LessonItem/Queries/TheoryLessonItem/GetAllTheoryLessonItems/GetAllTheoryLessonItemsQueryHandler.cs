namespace CourseService.Application.LessonItem.Queries.TheoryLessonItem.GetAllTheoryLessonItems;

public class GetAllTheoryLessonItemsQueryHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    IQueryContext queryContext,
    IMapper mapper,
    IRequestClient<GetSchoolProfilesRequest> getSchoolProfilesClient,
    IFilesManager filesManager
    ) : IRequestHandler<GetAllTheoryLessonItemsQuery, IEnumerable<TheoryLessonItemModelResponse>>
{
    private readonly IQueryContext _queryContext = queryContext;

    private readonly IMapper _mapper = mapper;

    private readonly IRequestClient<GetSchoolProfilesRequest> _getSchoolProfilesClient = getSchoolProfilesClient;

    private readonly IFilesManager _filesManager = filesManager;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    public async Task<IEnumerable<TheoryLessonItemModelResponse>> Handle(GetAllTheoryLessonItemsQuery request, CancellationToken cancellationToken)
    {
        var getActiveProfileRequest = new GetActiveSchoolProfileRequest(
            UserId: request.UserId,
            AllowedProfileTypes: [Constants.Teacher, Constants.ClassTeacher,
                Constants.SchoolAdmin, Constants.Student, Constants.Teacher]
        );

        var retrievingActiveProfileResult = await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);
        if (retrievingActiveProfileResult.IsRight)
            return [];

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null || activeProfile.SchoolId == null)
            return [];

        var lesson = await _queryContext.Lessons.FindAsync(request.LessonId, CancellationToken.None);
        if (lesson is null)
            return [];

        var dbQuery = _queryContext.TheoryLessonItems
            .Include(item => item.Attachments)
            .Where(item => item.LessonId == lesson.Id)
            .AsQueryable();

        if (activeProfile.Type == Constants.Teacher)
            dbQuery = dbQuery.IgnoreQueryFilters();

        if (activeProfile.Type == Constants.Student)
            dbQuery = dbQuery.Where(item => !item.IsArchived);

        var entities = await dbQuery.ToListAsync(cancellationToken);

        var authorsIds = entities
            .Where(item => item.AuthorId.HasValue)
            .Select(item => item.AuthorId!.Value)
            .Distinct()
            .ToArray();
        var authors = await GetAuthors(authorsIds, cancellationToken);

        try
        {
            var theoryItemsResponses = _mapper.Map<IEnumerable<TheoryLessonItemModelResponse>>(entities);
            foreach (var theory in theoryItemsResponses)
            {
                if (theory.AuthorId.HasValue)
                    theory.Author = authors.First(author => author.Id == theory.AuthorId);

                var attachments = entities.First(e => e.Id == theory.Id).Attachments;
                if (attachments != null)
                {
                    var attachmentsUrls = new List<string>();
                    foreach (var attachment in attachments)
                    {
                        var attachmentUrlResult = _filesManager.GetFile(attachment.Url);

                        var attachmentUrl = attachmentUrlResult.IsRight ? null : ((FileSuccess)attachmentUrlResult).Url;

                        if (attachmentUrl != null)
                            attachmentsUrls.Add(attachmentUrl);
                    }

                    theory.Attachments = attachmentsUrls;
                }
            }

            return theoryItemsResponses;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private async Task<IEnumerable<SchoolProfileContract>> GetAuthors
        (Guid[]? teachersIds, CancellationToken cancellationToken)
    {
        if (teachersIds == null || teachersIds.Length == 0)
            return [];

        var getTeachersRequest = new GetSchoolProfilesRequest(Ids: teachersIds, null, null, null, null);

        try
        {
            var response =
                await _getSchoolProfilesClient.GetResponse<GetSchoolProfilesResponse>(getTeachersRequest, cancellationToken);

            if (response.Message.Profiles == null)
                return [];

            return response.Message.Profiles;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while sending get profiles with values @Request", getTeachersRequest);
            return [];
        }
    }
}
