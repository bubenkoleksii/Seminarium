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
        var profile = request.UserId.HasValue ? await _schoolProfileManager.GetActiveProfile(request.UserId.Value) : null;
        if (profile is null)
            return new InvalidError("school_profile");

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

                    try
                    {
                        var allParents = await _queryContext.SchoolProfiles
                                    .Where(p => p.Type == SchoolProfileType.Parent)
                                    .ToListAsync(CancellationToken.None);

                        var allChildren = await _queryContext.SchoolProfiles
                            .Where(p => p.Type == SchoolProfileType.Student)
                            .ToListAsync(CancellationToken.None);

                        var filteredParents = allParents
                            .Where(p => allChildren.Exists(child => child.Id == entity.Id &&
                                                                 allParents != null &&
                                                                 allParents.Exists(parent => parent.Id == p.Id)))
                            .ToList();

                        entity.Parents = filteredParents;

                        var isNotParentAndChild = profile.Type == SchoolProfileType.Parent &&
                            (!filteredParents?.Exists(pa => pa.Id == profile.Id) ?? false);
                        if (isNotParentAndChild)
                            return new InvalidError("school_profile");

                        filteredParents?.ForEach(pa => pa.Children = null);

                        entity.Parents = filteredParents;
                        schoolProfileResponse.Parents = _mapper.Map<ICollection<SchoolProfileModelResponse>>(entity.Parents);
                    }
                    catch (Exception ex)
                    {
                        schoolProfileResponse.Parents = null;
                        Log.Error(ex, "Exception caught while retrieving parents of student with @Id", profile.Id);
                    }

                    break;
                }
            case { Type: SchoolProfileType.Parent }:
                {
                    try
                    {
                        var allParents = await _queryContext.SchoolProfiles
                            .Where(p => p.Type == SchoolProfileType.Parent)
                            .ToListAsync(CancellationToken.None);

                        var allChildren = await _queryContext.SchoolProfiles
                            .Where(p => p.Type == SchoolProfileType.Student)
                            .ToListAsync(CancellationToken.None);

                        var filteredChildren = allChildren
                            .Where(p => allParents.Exists(parent => parent.Id == entity.Id &&
                                                                 allChildren != null &&
                                                                 allChildren.Exists(child => child.Id == p.Id)))
                            .ToList();

                        entity.Children = filteredChildren;
                        schoolProfileResponse.Children =
                            _mapper.Map<ICollection<SchoolProfileModelResponse>>(entity.Children);
                    }
                    catch (Exception ex)
                    {
                        schoolProfileResponse.Children = null;
                        Log.Error(ex, "Exception caught while retrieving parents of student with @Id", profile.Id);
                    }

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
            var isNotSameUser = profile?.UserId != schoolProfileResponse.UserId;
            var isNotSameSchool = schoolProfileResponse.School?.Id != profile?.School?.Id;

            if (profile is null || (isNotSameUser && isNotSameSchool &&
                profile.Type != SchoolProfileType.Student &&
                profile.Type != SchoolProfileType.Parent))
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
                        schoolProfileResponse.TeacherSubjects = data?.TeachersSubjects;
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
