namespace CourseService.Application.LessonItem.Queries.PracticalLessonItem.GetAllStudentPracticalLessonItems;

public class GetAllStudentPracticalLessonItemsQueryHandler(
    IQueryContext queryContext,
    IRequestClient<GetSchoolProfilesRequest> getSchoolProfilesClient
    ) : IRequestHandler<GetAllStudentPracticalLessonItemsQuery, Either<GetAllStudentPracticalLessonItemsModelResponse, Error>>
{
    private const int DefaultTake = 8;

    private readonly IQueryContext _queryContext = queryContext;

    private readonly IRequestClient<GetSchoolProfilesRequest> _getSchoolProfilesClient = getSchoolProfilesClient;

    public async Task<Either<GetAllStudentPracticalLessonItemsModelResponse, Error>> Handle(
        GetAllStudentPracticalLessonItemsQuery request,
        CancellationToken cancellationToken)
    {
        var student = await GetStudent(request.StudentId, cancellationToken);
        if (student == null)
            return new InvalidError("student");

        var courses = await _queryContext.Courses
                .Include(c => c.Groups)
                .ToListAsync(cancellationToken);

        var filteredCourses = courses
            .Where(course => course.Groups != null && course.Groups
                .Any(group => group.Id == student.GroupId))
            .ToList();

        var coursesIds = filteredCourses.Select(c => c.Id);

        var lessons = await _queryContext.Lessons
            .Include(l => l.Course)
            .Where(l => coursesIds.Contains(l.CourseId))
            .ToListAsync(cancellationToken);

        var lessonsIds = lessons.Select(l => l.Id);

        var take = request.Take ?? DefaultTake;

        var items = await _queryContext.PracticalLessonItems
            .Where(item => lessonsIds.Contains(item.LessonId))
            .OrderByDescending(l => l.CreatedAt)
            .Skip((int)request.Skip)
            .Take((int)take)
            .ToListAsync(cancellationToken);

        if (items.Count == 0)
            return new NotFoundError("practical_item");

        items = [.. items.OrderByDescending(i => i.Deadline)];

        var entries = new List<StudentPracticalLessonItemModelResponse>();
        foreach (var item in items)
        {
            var entry = new StudentPracticalLessonItemModelResponse
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                Title = item.Title,
                Deadline = item.Deadline
            };

            var lesson = lessons.First(l => l.Id == item.LessonId);
            entry.LessonTopic = lesson.Topic;
            entry.CourseName = lesson.Course?.Name;

            var submit = await _queryContext.PracticalLessonItemSubmits
                .Where(submit => submit.PracticalLessonItemId == item.Id && submit.StudentId == student.Id)
                .OrderByDescending(submit => submit.Attempt)
                .FirstOrDefaultAsync(cancellationToken);
            if (submit != null)
                entry.Status = submit.Status;

            entries.Add(entry);
        }

        var response = new GetAllStudentPracticalLessonItemsModelResponse(
            Entries: entries,
            StudentId: request.StudentId,
            Total: (ulong)items.Count,
            Skip: request.Skip,
            Take: take
        );

        return response;
    }

    private async Task<SchoolProfileContract?> GetStudent(Guid studentId, CancellationToken cancellationToken)
    {
        var getStudentRequest = new GetSchoolProfilesRequest(Ids: [studentId], null, null, null, null);

        try
        {
            var response = await _getSchoolProfilesClient.GetResponse<GetSchoolProfilesResponse>(getStudentRequest, cancellationToken);

            if (response.Message.Profiles == null || !response.Message.Profiles.Any())
                return null;

            return response.Message.Profiles.First();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while sending get profiles request", getStudentRequest);
            return null;
        }
    }
}
