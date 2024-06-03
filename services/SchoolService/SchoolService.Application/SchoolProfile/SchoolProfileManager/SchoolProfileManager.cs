namespace SchoolService.Application.SchoolProfile.SchoolProfileManager;

public class SchoolProfileManager : ISchoolProfileManager
{
    private readonly ICommandContext _commandContext;

    private readonly IQueryContext _queryContext;

    private readonly IMemoryCache _memoryCache;

    private readonly IMapper _mapper;

    public SchoolProfileManager(
        ICommandContext commandContext,
        IQueryContext queryContext,
        IMemoryCache memoryCache,
        IMapper mapper)
    {
        _commandContext = commandContext;
        _queryContext = queryContext;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }

    public async Task<Either<Domain.Entities.SchoolProfile, Error>> CreateSchoolAdminProfile(
        Invitation invitation, CreateSchoolProfileCommand command)
    {
        if (DateTime.UtcNow > invitation.Expired)
            return new InvalidError("invitation");

        var school = await _commandContext.Schools.FindAsync(invitation.SourceId);
        if (school == null)
            return new InvalidError("school_id");

        var existedProfile = await _commandContext.SchoolProfiles
            .AsNoTracking()
            .Where(p => p.SchoolId == school.Id && p.UserId == command.UserId)
            .FirstOrDefaultAsync();

        if (existedProfile != null)
            return new AlreadyExistsError("school_profile")
            {
                Params = new List<string>(2) { "user_id", "school_id" }
            };

        var profile = _mapper.Map<Domain.Entities.SchoolProfile>(command);
        profile.School = school;
        profile.Type = invitation.Type;
        profile.IsActive = true;

        return profile;
    }

    public async Task<Either<Domain.Entities.SchoolProfile, Error>> CreateClassTeacherProfile(Invitation invitation, CreateSchoolProfileCommand command)
    {
        if (DateTime.UtcNow > invitation.Expired)
            return new InvalidError("invitation");

        var group = await _queryContext.Groups
            .Include(g => g.ClassTeacher)
            .FirstOrDefaultAsync(g => g.Id == invitation.SourceId);
        if (group == null)
            return new InvalidError("group_id");

        if (group.ClassTeacher is not null)
            return new AlreadyExistsError("class_teacher");

        var existedProfile = await _commandContext.SchoolProfiles
            .Where(p => p.GroupId == group.Id && p.UserId == command.UserId)
            .FirstOrDefaultAsync();

        if (existedProfile != null)
            return new AlreadyExistsError("school_profile")
            {
                Params = new List<string>(2) { "user_id", "group_id" }
            };

        var profile = _mapper.Map<Domain.Entities.SchoolProfile>(command);
        profile.ClassTeacherGroupId = group.Id;
        profile.Type = invitation.Type;
        profile.IsActive = true;

        return profile;
    }

    public async Task<Either<Domain.Entities.SchoolProfile, Error>> CreateTeacherProfile(
        Invitation invitation, CreateSchoolProfileCommand command)
    {
        if (DateTime.UtcNow > invitation.Expired)
            return new InvalidError("invitation");

        if (command.TeacherLessonsPerCycle is null)
            return new InvalidError("lessons_per_cycle");

        var school = await _commandContext.Schools.FindAsync(invitation.SourceId);
        if (school == null)
            return new InvalidError("school_id");

        var existedProfile = await _commandContext.SchoolProfiles
            .Where(p => p.SchoolId == school.Id && p.UserId == command.UserId)
            .FirstOrDefaultAsync();

        if (existedProfile != null)
            return new AlreadyExistsError("school_profile")
            {
                Params = new List<string>(2) { "user_id", "school_id" }
            };

        var profile = _mapper.Map<Domain.Entities.SchoolProfile>(command);

        var data = new TeacherSerializationData(
            command.TeacherExperience,
            command.TeacherEducation,
            command.TeacherQualification,
            (uint)command.TeacherLessonsPerCycle
        );

        profile.School = school;
        profile.Type = invitation.Type;
        profile.Data = JsonConvert.SerializeObject(data);
        profile.IsActive = true;

        return profile;
    }

    public async Task<Either<Domain.Entities.SchoolProfile, Error>> CreateStudentProfile(Invitation invitation, CreateSchoolProfileCommand command)
    {
        if (DateTime.UtcNow > invitation.Expired)
            return new InvalidError("invitation");

        if (command.StudentIsIndividually is null)
            return new InvalidError("is_individually");

        if (command.StudentIsClassLeader is null)
            return new InvalidError("is_class_leader");

        if (command.StudentHealthGroup is null)
            return new InvalidError("health_group");

        var group = await _queryContext.Groups.FindAsync(invitation.SourceId);
        if (group == null)
            return new InvalidError("group_id");

        var existedProfile = await _commandContext.SchoolProfiles
            .Where(p => p.GroupId == group.Id && p.UserId == command.UserId)
            .FirstOrDefaultAsync();

        if (existedProfile != null)
            return new AlreadyExistsError("school_profile")
            {
                Params = new List<string>(2) { "user_id", "group_id" }
            };

        var profile = _mapper.Map<Domain.Entities.SchoolProfile>(command);
        profile.GroupId = group.Id;
        profile.Type = invitation.Type;
        profile.IsActive = true;

        return profile;
    }

    public async Task<(Either<Domain.Entities.SchoolProfile, Error> Result, bool IsNew)> CreateParentProfileOrAddChild
        (Invitation invitation, CreateSchoolProfileCommand command)
    {
        if (DateTime.UtcNow > invitation.Expired)
            return (new InvalidError("invitation"), false);

        var child = await _commandContext.SchoolProfiles.FindAsync(invitation.SourceId);
        if (child is null)
            return (new InvalidError("child_id"), false);
        child.Parents ??= new List<Domain.Entities.SchoolProfile>();

        var existedParent = await _queryContext.SchoolProfiles
            .Include(p => p.Children)
            .FirstOrDefaultAsync(p => p.Id == command.UserId);

        if (existedParent is { Children: { } } && existedParent.Children.Contains(child))
            return (new AlreadyExistsError("child")
            {
                Params = new List<string>(2) { "child_id" }
            }, false);

        if (existedParent == null)
        {
            var serializationData = JsonConvert.SerializeObject(
                new ParentSerializationData(command.ParentAddress)
            );

            var profile = _mapper.Map<Domain.Entities.SchoolProfile>(command);

            profile.Type = invitation.Type;
            profile.IsActive = true;
            profile.Data = serializationData;

            await _commandContext.SchoolProfiles.AddAsync(profile);
            await _commandContext.SaveChangesAsync(CancellationToken.None);

            profile.Children ??= new List<Domain.Entities.SchoolProfile>();
            profile.Children.Add(child);
            child.Parents.Add(profile);

            try
            {
                await _commandContext.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "An error occurred while saving school parent profile with values {@Profile}.", profile);
            }


            return (profile, true);
        }

        existedParent.Children ??= new List<Domain.Entities.SchoolProfile>();

        existedParent.Children.Add(child);
        child.Parents.Add(existedParent);

        existedParent.IsActive = true;

        await _commandContext.SaveChangesAsync(CancellationToken.None);

        return (existedParent, false);
    }

    public async Task<SchoolProfileModelResponse?> CacheProfiles(Guid userId, Guid currentProfileId)
    {
        ClearCache(userId);
        var cacheKey = GetCacheKey(userId);

        var userProfiles = await _commandContext.SchoolProfiles
            .Where(profile => profile.UserId == userId)
            .ToListAsync();

        if (!userProfiles.Any())
        {
            _memoryCache.Set<IEnumerable<SchoolProfileModelResponse>?>(cacheKey, null);
            return null;
        }

        ActivateProfile(userProfiles, currentProfileId);

        var profilesToCache = await MapToResponses(userProfiles);
        _memoryCache.Set(cacheKey, profilesToCache);

        return profilesToCache?.FirstOrDefault(p => p.IsActive);
    }

    public async Task<IEnumerable<SchoolProfileModelResponse>?> GetProfiles(Guid userId)
    {
        var cacheKey = GetCacheKey(userId);

        if (_memoryCache.TryGetValue(cacheKey, out List<SchoolProfileModelResponse>? cachedProfiles))
            return cachedProfiles;

        var profiles = await _queryContext.SchoolProfiles
            .Where(profile => profile.UserId == userId)
            .ToListAsync();

        var profileResponses = await MapToResponses(profiles);
        _memoryCache.Set(cacheKey, profileResponses);
        return profileResponses;
    }

    public async Task<SchoolProfileModelResponse?> GetActiveProfile(Guid userId)
    {
        var profiles = await GetProfiles(userId);

        var activeProfile = profiles?.FirstOrDefault(p => p.IsActive);
        return activeProfile;
    }

    public async Task<Option<Error>> ValidateSchoolProfileBySchool(Guid? userId, Guid schoolId)
    {
        if (userId is null)
            return Option<Error>.None;

        var schoolProfile = await GetActiveProfile((Guid)userId);

        if (schoolProfile?.SchoolId is null)
            return new InvalidError("school_profile");

        var profileSchoolId = (Guid)schoolProfile.SchoolId;
        return profileSchoolId != schoolId
            ? new InvalidError("school")
            : Option<Error>.None;
    }

    public async Task<Option<Error>> ValidateSchoolProfileByGroup(Guid? userId, Guid groupId)
    {
        if (userId is null)
            return Option<Error>.None;

        var schoolProfile = await GetActiveProfile((Guid)userId);

        if (schoolProfile?.GroupId is null)
            return new InvalidError("school_profile");

        var profileGroupId = (Guid)schoolProfile.GroupId;
        return profileGroupId != groupId
            ? new InvalidError("group")
            : Option<Error>.None;
    }

    public async Task<Option<Error>> ValidateClassTeacherSchoolProfileByGroup(Guid? userId, Guid groupId)
    {
        if (userId is null)
            return Option<Error>.None;

        var schoolProfile = await GetActiveProfile((Guid)userId);

        if (schoolProfile?.ClassTeacherGroupId is null)
            return new InvalidError("school_profile");

        var profileClassTeacherGroupId = (Guid)schoolProfile.ClassTeacherGroupId;
        return profileClassTeacherGroupId != groupId
            ? new InvalidError("school_id")
            : Option<Error>.None;
    }

    public void ClearCache(Guid userId)
    {
        var cacheKey = GetCacheKey(userId);
        _memoryCache.Remove(cacheKey);
    }

    private static void ActivateProfile(List<Domain.Entities.SchoolProfile> profiles, Guid currentProfileId)
        => profiles.ForEach(profile => profile.IsActive = profile.Id == currentProfileId);

    private async Task<IReadOnlyList<SchoolProfileModelResponse>?> MapToResponses(IEnumerable<Domain.Entities.SchoolProfile>? entities)
    {
        if (entities is null)
            return null;

        var responses = new List<SchoolProfileModelResponse>();

        foreach (var entity in entities)
        {
            var response = _mapper.Map<SchoolProfileModelResponse>(entity);

            switch (entity)
            {
                case { Type: SchoolProfileType.SchoolAdmin }:
                    {
                        var school = await _queryContext.Schools.FindAsync(entity.SchoolId);
                        response.SchoolName = school?.Name;
                        break;
                    }
            }

            if (string.IsNullOrWhiteSpace(entity.Data))
            {
                responses.Add(response);
                continue;
            }

            switch (entity)
            {
                case { Type: SchoolProfileType.Parent }:
                    {
                        var data = JsonConvert.DeserializeObject<ParentSerializationData>(entity.Data);
                        response.ParentAddress = data?.ParentAddress;
                        break;
                    }
                case { Type: SchoolProfileType.Teacher }:
                    {
                        var data = JsonConvert.DeserializeObject<TeacherSerializationData>(entity.Data);
                        response.TeacherEducation = data?.TeachersEducation;
                        response.TeacherQualification = data?.TeachersQualification;
                        response.TeacherExperience = data?.TeachersExperience;
                        response.TeacherLessonsPerCycle = data?.TeachersLessonsPerCycle;
                        break;
                    }
                case { Type: SchoolProfileType.Student }:
                    {
                        var data = JsonConvert.DeserializeObject<StudentSerializationData>(entity.Data);
                        response.StudentDateOfBirth = data?.StudentDateOfBirth;
                        response.StudentAptitudes = data?.StudentAptitudes;
                        response.StudentIsClassLeader = data?.StudentIsClassLeader;
                        response.StudentIsIndividually = data?.StudentIsIndividually;
                        response.StudentHealthGroup = data?.StudentHealthGroup;
                        break;
                    }
            }

            responses.Add(response);
        }

        return responses.Any() ? responses : null;
    }

    private static string GetCacheKey(Guid userId)
    {
        return $"SchoolProfiles_{userId}";
    }
}
