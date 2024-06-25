namespace CourseService.Application.LessonItem.Queries.PracticalLessonItem.GetAllPracticalLessonItems;

public class GetAllPracticalLessonItemsQueryHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    IQueryContext queryContext,
    IMapper mapper,
    IRequestClient<GetSchoolProfilesRequest> getSchoolProfilesClient,
    IFilesManager filesManager
    ) : IRequestHandler<GetAllPracticalLessonItemsQuery, IEnumerable<PracticalLessonItemModelResponse>>
{
    private readonly IQueryContext _queryContext = queryContext;

    private readonly IMapper _mapper = mapper;

    private readonly IRequestClient<GetSchoolProfilesRequest> _getSchoolProfilesClient = getSchoolProfilesClient;

    private readonly IFilesManager _filesManager = filesManager;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    public async Task<IEnumerable<PracticalLessonItemModelResponse>> Handle(GetAllPracticalLessonItemsQuery request, CancellationToken cancellationToken)
    {
        var getActiveProfileRequest = new GetActiveSchoolProfileRequest(
            UserId: request.UserId,
            AllowedProfileTypes: [Constants.Teacher, Constants.ClassTeacher, Constants.SchoolAdmin, Constants.Student, Constants.Teacher]
        );

        var retrievingActiveProfileResult = await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);
        if (retrievingActiveProfileResult.IsRight)
            return [];

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null || activeProfile.SchoolId == null)
            return Enumerable.Empty<PracticalLessonItemModelResponse>();

        var lesson = await _queryContext.Lessons.FindAsync(request.LessonId, cancellationToken);
        if (lesson == null)
            return Enumerable.Empty<PracticalLessonItemModelResponse>();

        var dbQuery = _queryContext.PracticalLessonItems
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
            var practicalItemsResponses = _mapper.Map<IEnumerable<PracticalLessonItemModelResponse>>(entities);
            foreach (var practical in practicalItemsResponses)
            {
                if (practical.AuthorId.HasValue)
                    practical.Author = authors.First(author => author.Id == practical.AuthorId);

                var attachments = entities.First(e => e.Id == practical.Id)?.Attachments;
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

                    practical.Attachments = attachmentsUrls;
                }
            }

            return practicalItemsResponses;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return [];
        }
    }

    private async Task<IEnumerable<SchoolProfileContract>> GetAuthors(Guid[]? teachersIds, CancellationToken cancellationToken)
    {
        if (teachersIds == null || teachersIds.Length == 0)
            return [];

        var getTeachersRequest = new GetSchoolProfilesRequest(Ids: teachersIds, null, null, null, null);

        try
        {
            var response = await _getSchoolProfilesClient.GetResponse<GetSchoolProfilesResponse>(getTeachersRequest, cancellationToken);

            if (response.Message.Profiles == null)
                return [];

            return response.Message.Profiles;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while sending get profiles request", getTeachersRequest);
            return [];
        }
    }
}
