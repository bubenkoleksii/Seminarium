using Exception = System.Exception;

namespace SchoolService.Application.SchoolProfile.SchoolProfileManager;

public class SchoolProfileManager : ISchoolProfileManager
{
    private readonly ICommandContext _commandContext;

    private readonly IQueryContext _queryContext;

    private readonly IMemoryCache _memoryCache;

    private readonly IMapper _mapper;

    private readonly IFilesManager _filesManager;

    public SchoolProfileManager(
        ICommandContext commandContext,
        IQueryContext queryContext,
        IMemoryCache memoryCache,
        IMapper mapper,
        IFilesManager filesManager)
    {
        _commandContext = commandContext;
        _queryContext = queryContext;
        _memoryCache = memoryCache;
        _mapper = mapper;
        _filesManager = filesManager;
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
            .Where(p => p.SchoolId == school.Id && p.UserId == command.UserId &&
                p.Type == SchoolProfileType.SchoolAdmin)
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
            .Where(p => p.GroupId == group.Id && p.UserId == command.UserId &&
                        p.Type == SchoolProfileType.ClassTeacher)
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
            .Where(p => p.SchoolId == school.Id && p.UserId == command.UserId &&
                p.Type == SchoolProfileType.Teacher)
            .FirstOrDefaultAsync();

        if (existedProfile != null)
            return new AlreadyExistsError("school_profile")
            {
                Params = new List<string>(2) { "user_id", "school_id" }
            };

        var profile = _mapper.Map<Domain.Entities.SchoolProfile>(command);

        var data = new TeacherSerializationData(
            command.TeacherSubjects,
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
            .Where(p => p.GroupId == group.Id && p.UserId == command.UserId &&
                        p.Type == SchoolProfileType.Student)
            .FirstOrDefaultAsync();

        if (existedProfile != null)
            return new AlreadyExistsError("school_profile")
            {
                Params = new List<string>(2) { "user_id", "group_id" }
            };

        var data = new StudentSerializationData(
            command.StudentDateOfBirth,
            command.StudentAptitudes,
            (bool)command.StudentIsClassLeader,
            (bool)command.StudentIsIndividually,
            (HealthGroup)command.StudentHealthGroup
        );

        var serializationData = JsonConvert.SerializeObject(data);

        var profile = _mapper.Map<Domain.Entities.SchoolProfile>(command);
        profile.GroupId = group.Id;
        profile.Type = invitation.Type;
        profile.IsActive = true;
        profile.Data = serializationData;

        return profile;
    }

    public async Task<(Either<Domain.Entities.SchoolProfile, Error> Result, bool IsNew)> CreateParentProfileOrAddChild
        (Invitation invitation, CreateSchoolProfileCommand command)
    {
        if (DateTime.UtcNow > invitation.Expired)
            return (new InvalidError("invitation"), false);

        var child = await _commandContext
            .SchoolProfiles
            .FindAsync(invitation.SourceId);
        if (child is null)
            return (new InvalidError("child_id"), false);

        var existedParent = await _commandContext.SchoolProfiles
            .Include(p => p.Children)
            .FirstOrDefaultAsync(p => p.UserId == command.UserId && p.Type == SchoolProfileType.Parent);

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

            _commandContext.SchoolProfiles.Update(child);
            _commandContext.SchoolProfiles.Update(profile);

            ClearCache(child.UserId);

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
        foreach (var childToClearCache in existedParent.Children)
            ClearCache(childToClearCache.UserId);

        existedParent.IsActive = true;

        try
        {
            _commandContext.SchoolProfiles.Update(existedParent);

            await _commandContext.SaveChangesAsync(CancellationToken.None);
            return (existedParent, false);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Excpetion caught while adding child to parent with id @Id", existedParent.UserId);
            return (new InvalidDatabaseOperationError("school_profile"), false);
        }
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

        foreach (var profile in profiles)
        {
            switch (profile.Type)
            {
                case SchoolProfileType.SchoolAdmin or SchoolProfileType.Teacher:
                    {
                        var school = await _queryContext.Schools.FindAsync(profile.SchoolId);
                        profile.School = school;
                        break;
                    }
                case SchoolProfileType.ClassTeacher:
                    {
                        var group = await _queryContext.Groups
                            .Include(g => g.School)
                            .FirstOrDefaultAsync(g => g.Id == profile.ClassTeacherGroupId);
                        profile.Group = group;
                        profile.School = group?.School;
                        break;
                    }
                case SchoolProfileType.Student:
                    {
                        var group = await _queryContext.Groups
                            .Include(g => g.School)
                            .FirstOrDefaultAsync(g => g.Id == profile.GroupId);
                        profile.Group = group;
                        profile.School = group?.School;

                        try
                        {
                            var allParents = await _queryContext.SchoolProfiles
                                        .Where(p => p.Type == SchoolProfileType.Parent)
                                        .ToListAsync(CancellationToken.None);

                            var allChildren = await _queryContext.SchoolProfiles
                                .Where(p => p.Type == SchoolProfileType.Student)
                                .ToListAsync(CancellationToken.None);

                            var filteredParents = allParents
                                .Where(p => allChildren.Exists(child => child.Id == profile.Id &&
                                                                     allParents != null &&
                                                                     allParents.Exists(parent => parent.Id == p.Id)))
                                .ToList();

                            profile.Parents = filteredParents;
                        }
                        catch
                        {
                            profile.Parents = null;
                        }

                        break;
                    }
                case SchoolProfileType.Parent:
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
                                .Where(p => allParents.Exists(parent => parent.Id == profile.Id &&
                                                                     allChildren != null &&
                                                                     allChildren.Exists(child => child.Id == p.Id)))
                                .ToList();

                            profile.Children = filteredChildren;
                        }
                        catch
                        {
                            profile.Children = null;
                        }

                        break;
                    }
            }
        }

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

    public async Task<Option<Error>> ValidateParentProfileByChildGroup(Guid? userId, Guid groupId)
    {
        if (userId is null)
            return Option<Error>.None;

        var profile = await GetActiveProfile((Guid)userId);
        if (profile is null)
            return new InvalidError("school_profile");

        try
        {
            var allParents = await _queryContext.SchoolProfiles
                .Where(p => p.Type == SchoolProfileType.Parent)
                .ToListAsync(CancellationToken.None);

            var allChildren = await _queryContext.SchoolProfiles
                .Where(p => p.Type == SchoolProfileType.Student)
                .ToListAsync(CancellationToken.None);

            var filteredChildren = allChildren
                .Where(p => allParents.Exists(parent => parent.Id == profile.Id &&
                                                     allChildren != null &&
                                                     allChildren.Exists(child => child.Id == p.Id)));

            var filteredByGroupChildren = filteredChildren
                .Where(p => p.GroupId == groupId)
                .ToList();

            if (filteredByGroupChildren.Count == 0)
                return new InvalidError("school_profile");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving children of parent with values {@Profile}.", profile);
            return new InvalidError("school_profile");
        }

        return Option<Error>.None;
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

            if (entity.Img is not null)
            {
                var image = _filesManager.GetFile(entity.Img);
                response.Img = image.IsRight ? null : ((FileSuccess)image).Url;
            }

            switch (entity)
            {
                case { Type: SchoolProfileType.SchoolAdmin or SchoolProfileType.Teacher }:
                    {
                        var school = await _queryContext.Schools.FindAsync(entity.SchoolId);
                        response.SchoolName = school?.Name;
                        response.SchoolId = school?.Id;
                        break;
                    }
                case { Type: SchoolProfileType.Student }:
                    {
                        var group = await _queryContext.Groups
                            .Include(group => group.School)
                            .FirstOrDefaultAsync(g => g.Id == entity.GroupId);
                        response.SchoolName = group?.School.Name;
                        response.SchoolId = group?.School.Id;
                        break;
                    }
                case { Type: SchoolProfileType.ClassTeacher }:
                    {
                        var group = await _queryContext.Groups
                            .Include(group => group.School)
                            .FirstOrDefaultAsync(g => g.Id == entity.ClassTeacherGroupId);
                        response.SchoolName = group?.School.Name;
                        response.SchoolId = group?.School.Id;
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
                        response.TeacherSubjects = data?.TeachersSubjects;
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
