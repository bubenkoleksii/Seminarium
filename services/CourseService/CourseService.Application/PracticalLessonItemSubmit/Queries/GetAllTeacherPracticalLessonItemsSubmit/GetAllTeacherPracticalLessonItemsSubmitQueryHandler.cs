namespace CourseService.Application.PracticalLessonItemSubmit.Queries.GetAllTeacherPracticalLessonItemsSubmit;

public class GetAllTeacherPracticalLessonItemsSubmitQueryHandler(
    IQueryContext queryContext,
    IRequestClient<GetSchoolProfilesRequest> getSchoolProfilesClient
    ) : IRequestHandler<GetAllTeacherPracticalLessonItemsSubmitQuery, Either<GetAllPracticalLessonItemSubmitModelResponse, Error>>
{
    private const int DefaultTake = 8;

    private readonly IQueryContext _queryContext = queryContext;

    private readonly IRequestClient<GetSchoolProfilesRequest> _getSchoolProfilesClient = getSchoolProfilesClient;

    public async Task<Either<GetAllPracticalLessonItemSubmitModelResponse, Error>> Handle(GetAllTeacherPracticalLessonItemsSubmitQuery request, CancellationToken cancellationToken)
    {
        var take = request.Take ?? DefaultTake;

        var submits = await _queryContext.PracticalLessonItemSubmits
            .Where(s => s.PracticalLessonItemId == request.ItemId)
            .Skip((int)request.Skip)
            .Take((int)take)
            .ToListAsync(cancellationToken);

        var studentIds = submits.Select(s => s.StudentId).ToArray();

        var getStudentRequest = new GetSchoolProfilesRequest(Ids: studentIds, UserId: null, GroupId: null, SchoolId: null, Type: null);
        var studentResponse = await _getSchoolProfilesClient.GetResponse<GetSchoolProfilesResponse>(getStudentRequest, cancellationToken);
        if (studentResponse.Message.HasError)
            return (Error)studentResponse;

        var studentContracts = studentResponse.Message.Profiles;
        if (studentContracts == null || !studentContracts.Any())
            return new InvalidError("students");

        var studentDict = studentContracts.ToDictionary(s => s.Id, s => s);

        var responseEntries = new List<GetAllPracticalLessonItemSubmitModelResponseItem>();

        foreach (var submit in submits)
        {
            var studentContract = studentDict[submit.StudentId];

            var responseItem = new GetAllPracticalLessonItemSubmitModelResponseItem
            {
                Id = submit.Id,
                CreatedAt = submit.CreatedAt,
                StudentId = submit.StudentId,
                StudentName = studentContract.Name,
                PracticalLessonItemId = submit.PracticalLessonItemId,
                Status = submit.Status
            };

            responseEntries.Add(responseItem);
        }

        var response = new GetAllPracticalLessonItemSubmitModelResponse(
            Entries: responseEntries,
            Total: (ulong)responseEntries.Count,
            Skip: request.Skip,
            Take: take
        );

        return response;
    }
}
