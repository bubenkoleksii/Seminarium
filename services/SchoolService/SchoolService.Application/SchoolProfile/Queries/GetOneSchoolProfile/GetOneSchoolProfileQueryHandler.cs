namespace SchoolService.Application.SchoolProfile.Queries.GetOneSchoolProfile;

public class GetOneSchoolProfileQueryHandler : IRequestHandler<GetOneSchoolProfileQuery, Either<SchoolProfileModelResponse, Error>>
{
    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IFilesManager _filesManager;

    public GetOneSchoolProfileQueryHandler(
        IQueryContext queryContext,
        IMapper mapper,
        ISchoolProfileManager schoolProfileManager,
        IFilesManager filesManager)
    {
        _queryContext = queryContext;
        _mapper = mapper;
        _schoolProfileManager = schoolProfileManager;
        _filesManager = filesManager;
    }

    public async Task<Either<SchoolProfileModelResponse, Error>> Handle(GetOneSchoolProfileQuery request, CancellationToken cancellationToken)
    {
        var entity = await _queryContext.SchoolProfiles
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.Id, "school_profile");

        var schoolProfileResponse = _mapper.Map<SchoolProfileModelResponse>(entity);
        if (entity.Img is not null)
        {
            var image = _filesManager.GetFile(entity.Img);
            schoolProfileResponse.Img = image.IsRight ? null : ((FileSuccess)image).Url;
        }

        switch (entity)
        {
            case { Type: SchoolProfileType.SchoolAdmin or SchoolProfileType.Teacher }:
                {
                    var school = await _queryContext.Schools.FindAsync(entity.SchoolId);

                    schoolProfileResponse.SchoolName = school?.Name;
                    schoolProfileResponse.SchoolId = school?.Id;
                    schoolProfileResponse.School = _mapper.Map<SchoolModelResponse>(school);
                    break;
                }
            case { Type: SchoolProfileType.Student }:
                {
                    var group = await _queryContext.Groups
                        .Include(group => group.School)
                        .FirstOrDefaultAsync(g => g.Id == entity.GroupId, cancellationToken);

                    schoolProfileResponse.SchoolName = group?.School.Name;
                    schoolProfileResponse.SchoolId = group?.School.Id;
                    schoolProfileResponse.School = _mapper.Map<SchoolModelResponse>(group?.School);
                    schoolProfileResponse.Group = _mapper.Map<GroupModelResponse>(group);

                    var parents = await _queryContext.SchoolProfiles
                        .Include(p => p.Children)
                        .Where(p => p.Type == SchoolProfileType.Parent)
                        .ToListAsync(CancellationToken.None);

                    entity.Parents = parents
                        .Where(p => p.Children != null &&
                                    p.Children.Any(child => child.Id == entity.Id))
                        .ToList();

                    schoolProfileResponse.Parents = _mapper.Map<IEnumerable<SchoolProfileModelResponse>>(entity.Parents);
                    break;
                }
            case { Type: SchoolProfileType.Parent }:
                {
                    var children = await _queryContext.SchoolProfiles
                        .Include(p => p.Parents)
                        .Where(p => p.Type == SchoolProfileType.Student)
                        .ToListAsync(CancellationToken.None);

                    entity.Children = children
                        .Where(p => p.Parents != null &&
                                    p.Parents.Any(parent => parent.Id == entity.Id))
                        .ToList();

                    schoolProfileResponse.Children =
                        _mapper.Map<IEnumerable<SchoolProfileModelResponse>>(entity.Children);
                    break;
                }
            case { Type: SchoolProfileType.ClassTeacher }:
                {
                    var group = await _queryContext.Groups
                        .Include(group => group.School)
                        .FirstOrDefaultAsync(g => g.Id == entity.ClassTeacherGroupId, cancellationToken);

                    schoolProfileResponse.SchoolName = group?.School.Name;
                    schoolProfileResponse.SchoolId = group?.School.Id;
                    schoolProfileResponse.School = _mapper.Map<SchoolModelResponse>(group?.School);
                    schoolProfileResponse.Group = _mapper.Map<GroupModelResponse>(group);
                    break;
                }
        }

        if (request.UserId.HasValue)
        {
            var profile = await _schoolProfileManager.GetActiveProfile(request.UserId.Value);

            var isNotSameSchool = schoolProfileResponse.School?.Id != profile?.School?.Id;
            var isNotParentAndChild = !(schoolProfileResponse.Children?.Any(c => c.Id == entity.Id) ?? false)
                                      && (!schoolProfileResponse.Parents?.Any(pa => pa.Id == entity.Id) ?? false);

            if (profile is null || (isNotSameSchool && isNotParentAndChild))
                return new InvalidError("school_profile");
        }

        if (entity.Data != null)
        {
            switch (entity)
            {
                case { Type: SchoolProfileType.Parent }:
                    {
                        var data = JsonConvert.DeserializeObject<ParentSerializationData>(entity.Data);
                        schoolProfileResponse.ParentAddress = data?.ParentAddress;
                        break;
                    }
                case { Type: SchoolProfileType.Teacher }:
                    {
                        var data = JsonConvert.DeserializeObject<TeacherSerializationData>(entity.Data);
                        schoolProfileResponse.TeacherEducation = data?.TeachersEducation;
                        schoolProfileResponse.TeacherQualification = data?.TeachersQualification;
                        schoolProfileResponse.TeacherExperience = data?.TeachersExperience;
                        schoolProfileResponse.TeacherLessonsPerCycle = data?.TeachersLessonsPerCycle;
                        break;
                    }
                case { Type: SchoolProfileType.Student }:
                    {
                        var data = JsonConvert.DeserializeObject<StudentSerializationData>(entity.Data);
                        schoolProfileResponse.StudentDateOfBirth = data?.StudentDateOfBirth;
                        schoolProfileResponse.StudentAptitudes = data?.StudentAptitudes;
                        schoolProfileResponse.StudentIsClassLeader = data?.StudentIsClassLeader;
                        schoolProfileResponse.StudentIsIndividually = data?.StudentIsIndividually;
                        schoolProfileResponse.StudentHealthGroup = data?.StudentHealthGroup;
                        break;
                    }
            }
        }

        return schoolProfileResponse;
    }
}
