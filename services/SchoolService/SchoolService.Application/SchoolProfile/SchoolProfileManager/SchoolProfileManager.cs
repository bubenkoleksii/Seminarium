namespace SchoolService.Application.SchoolProfile.SchoolProfileManager;

public class SchoolProfileManager : ISchoolProfileManager
{
    private readonly ICommandContext _commandContext;

    private readonly IQueryContext _queryContext;

    private readonly IMemoryCache _memoryCache;

    private readonly IMapper _mapper;

    public SchoolProfileManager(ICommandContext commandContext, IQueryContext queryContext, IMemoryCache memoryCache, IMapper mapper)
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

        return profile;
    }

    public async Task<Either<Domain.Entities.SchoolProfile, Error>> CreateTeacherProfile(
        Invitation invitation, CreateSchoolProfileCommand command)
    {
        if (DateTime.UtcNow > invitation.Expired)
            return new InvalidError("invitation");

        if (command.TeachersLessonsPerCycle is null)
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

        var data = new TeacherSerializationData(command.TeachersExperience,
            command.TeachersEducation,
            command.TeachersQualification,
            (uint)command.TeachersLessonsPerCycle
        );

        profile.School = school;
        profile.Type = invitation.Type;
        profile.Data = JsonConvert.SerializeObject(data);

        return profile;
    }

    public async Task<bool> CacheProfiles(Guid userId, Guid currentProfileId)
    {
        var cacheKey = GetCacheKey(userId);

        var userProfiles = await _commandContext.SchoolProfiles
            .Where(profile => profile.UserId == userId)
            .ToListAsync();

        if (!userProfiles.Any())
        {
            _memoryCache.Set<IEnumerable<SchoolProfileModelResponse>?>(cacheKey, null);
            return false;
        }

        userProfiles.ForEach(profile => profile.IsActive = profile.Id == currentProfileId);

        var profilesToCache = _mapper.Map<IEnumerable<SchoolProfileModelResponse>>(userProfiles);

        _memoryCache.Set(cacheKey, profilesToCache);

        return true;
    }

    public async Task<IEnumerable<SchoolProfileModelResponse>?> GetProfiles(Guid userId)
    {
        var cacheKey = GetCacheKey(userId);

        if (_memoryCache.TryGetValue(cacheKey, out List<Domain.Entities.SchoolProfile>? cachedProfiles))
            return cachedProfiles?.Select(_mapper.Map<SchoolProfileModelResponse>);

        var profiles = await _queryContext.SchoolProfiles
            .Where(profile => profile.UserId == userId)
            .ToListAsync();

        var profileResponses = profiles.Select(_mapper.Map<SchoolProfileModelResponse>);
        _memoryCache.Set(cacheKey, profiles);

        return profileResponses;
    }

    public async Task<SchoolProfileModelResponse?> GetActiveProfile(Guid userId)
    {
        var profiles = await GetProfiles(userId);

        var activeProfile = profiles?.FirstOrDefault(p => p.IsActive);
        return activeProfile;
    }

    public void ClearCache(Guid userId)
    {
        var cacheKey = GetCacheKey(userId);
        _memoryCache.Remove(cacheKey);
    }

    private static string GetCacheKey(Guid userId)
    {
        return $"SchoolProfiles_{userId}";
    }
}
