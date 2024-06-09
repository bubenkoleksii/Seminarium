namespace SchoolService.Application.Group.Queries.GetOneGroup;

public class GetOneGroupQueryHandler : IRequestHandler<GetOneGroupQuery, Either<OneGroupModelResponse, Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    private readonly IFilesManager _filesManager;

    public GetOneGroupQueryHandler(
        ISchoolProfileManager schoolProfileManager,
        IQueryContext queryContext,
        IMapper mapper,
        IFilesManager filesManager)
    {
        _schoolProfileManager = schoolProfileManager;
        _queryContext = queryContext;
        _mapper = mapper;
        _filesManager = filesManager;
    }

    public async Task<Either<OneGroupModelResponse, Error>> Handle(GetOneGroupQuery request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null)
            return new InvalidError("school_profile");

        var group = await _queryContext.Groups
            .Include(g => g.School)
            .Include(g => g.Students)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken: cancellationToken);

        if (group is null)
            return new NotFoundByIdError(request.Id, "group");

        var classTeacher = await _queryContext.SchoolProfiles
            .FirstOrDefaultAsync(p => p.Type == SchoolProfileType.ClassTeacher &&
            p.ClassTeacherGroupId == group.Id, cancellationToken: cancellationToken);

        if (classTeacher != null)
            classTeacher.SchoolId = group.SchoolId;

        group.ClassTeacher = classTeacher;

        switch (profile.Type)
        {
            case SchoolProfileType.SchoolAdmin or SchoolProfileType.Teacher:
                {
                    var validationError =
                        await _schoolProfileManager.ValidateSchoolProfileBySchool(request.UserId, group.SchoolId);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.Student:
                {
                    var validationError = await _schoolProfileManager.ValidateSchoolProfileByGroup(request.UserId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.ClassTeacher:
                {
                    var validationError = await _schoolProfileManager.ValidateClassTeacherSchoolProfileByGroup(request.UserId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.Parent:
                {
                    var validationError = await _schoolProfileManager.ValidateParentProfileByChildGroup(request.UserId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
        }

        group.Students = group.Students?
            .OrderBy(s => s.Name)
            .ToList();

        var lastNotice = await _queryContext.GroupNotices
            .OrderByDescending(n => n.CreatedAt)
            .FirstOrDefaultAsync(n => n.GroupId == group.Id, cancellationToken: cancellationToken);
        var lastNoticeResponse = _mapper.Map<GroupNoticeModelResponse>(lastNotice);

        var groupResponse = _mapper.Map<OneGroupModelResponse>(group);
        groupResponse.SchoolName = group.School.Name;
        groupResponse.LastNotice = lastNoticeResponse;

        var students = group.Students?.Select(student =>
        {
            var studentResponse = _mapper.Map<SchoolProfileModelResponse>(student);
            if (student.Img is not null)
            {
                var image = _filesManager.GetFile(student.Img);
                studentResponse.Img = image.IsRight ? null : ((FileSuccess)image).Url;
            }

            if (student.Data == null) return studentResponse;

            var data = JsonConvert.DeserializeObject<StudentSerializationData>(student.Data);
            studentResponse.StudentDateOfBirth = data?.StudentDateOfBirth;
            studentResponse.StudentAptitudes = data?.StudentAptitudes;
            studentResponse.StudentIsClassLeader = data?.StudentIsClassLeader;
            studentResponse.StudentIsIndividually = data?.StudentIsIndividually;
            studentResponse.StudentHealthGroup = data?.StudentHealthGroup;

            return studentResponse;
        });
        groupResponse.Students = students;

        if (group.Img is not null)
        {
            var image = _filesManager.GetFile(group.Img);
            groupResponse.Img = image.IsRight ? null : ((FileSuccess)image).Url;
        }

        return groupResponse;
    }
}
