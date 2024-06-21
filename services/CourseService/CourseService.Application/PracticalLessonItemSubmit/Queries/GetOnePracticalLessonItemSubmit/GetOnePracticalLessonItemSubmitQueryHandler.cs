namespace CourseService.Application.PracticalLessonItemSubmit.Queries.GetOnePracticalLessonItemSubmit;

public class GetOnePracticalLessonItemSubmitQueryHandler(
    IRequestClient<GetSchoolProfilesRequest> getSchoolProfilesClient,
    IQueryContext queryContext,
    IFilesManager filesManager,
    IMapper mapper
    ) : IRequestHandler<GetOnePracticalLessonItemSubmitQuery, Either<PracticalLessonItemSubmitModelResponse?, Error>>
{
    private readonly IRequestClient<GetSchoolProfilesRequest> _getSchoolProfilesClient = getSchoolProfilesClient;

    private readonly IQueryContext _queryContext = queryContext;

    private readonly IMapper _mapper = mapper;

    private readonly IFilesManager _filesManager = filesManager;

    public async Task<Either<PracticalLessonItemSubmitModelResponse?, Error>> Handle(GetOnePracticalLessonItemSubmitQuery request, CancellationToken cancellationToken)
    {
        var getStudentRequest = new GetSchoolProfilesRequest(Ids: [request.StudentId], UserId: null, GroupId: null, SchoolId: null, Type: null);

        var studentResponse = await _getSchoolProfilesClient.GetResponse<GetSchoolProfilesResponse>(getStudentRequest, cancellationToken);
        if (studentResponse.Message.HasError)
            return (Error)studentResponse;

        var studentContract = studentResponse.Message.Profiles?.FirstOrDefault();
        if (studentContract == null)
            return new InvalidError("student");

        var submit = await _queryContext.PracticalLessonItemSubmits
            .Include(item => item.Attachments)
            .FirstOrDefaultAsync(item => item.PracticalLessonItemId == request.ItemId && item.StudentId == request.StudentId, cancellationToken);
        if (submit == null)
            return new NotFoundError("submit");

        var practiceLessonItemSubmitModelResponse = _mapper.Map<PracticalLessonItemSubmitModelResponse>(submit);

        if (submit.Attachments != null)
        {
            var attachmentsUrls = new List<string>();
            foreach (var attachment in submit.Attachments)
            {
                var attachmentUrlResult = _filesManager.GetFile(attachment.Url);

                var attachmentUrl = attachmentUrlResult.IsRight ? null : ((FileSuccess)attachmentUrlResult).Url;

                if (attachmentUrl != null)
                    attachmentsUrls.Add(attachmentUrl);
            }

            practiceLessonItemSubmitModelResponse.Attachments = attachmentsUrls;
            practiceLessonItemSubmitModelResponse.StudentName = studentContract.Name;
        }

        return practiceLessonItemSubmitModelResponse;
    }
}
