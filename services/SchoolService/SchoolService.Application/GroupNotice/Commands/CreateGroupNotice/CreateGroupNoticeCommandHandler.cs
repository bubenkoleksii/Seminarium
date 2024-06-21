namespace SchoolService.Application.GroupNotice.Commands.CreateGroupNotice;

public class CreateGroupNoticeCommandHandler(ISchoolProfileManager schoolProfileManager,
    IMapper mapper, ICommandContext commandContext, IMailService mailService, IConfiguration configuration)
    : IRequestHandler<CreateGroupNoticeCommand, Either<GroupNoticeModelResponse, Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly IMapper _mapper = mapper;

    private readonly ICommandContext _commandContext = commandContext;

    private readonly IMailService _mailService = mailService;

    private readonly IConfiguration _configuration = configuration;

    public async Task<Either<GroupNoticeModelResponse, Error>> Handle(CreateGroupNoticeCommand request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null)
            return new InvalidError("school_profile");

        var group = await _commandContext.Groups.FindAsync(request.GroupId, CancellationToken.None);
        if (group is null)
            return new InvalidError("group_id");

        var validationResult = await ValidateProfile(profile.Type, profile.UserId, group);
        if (validationResult.IsSome)
            return (Error)validationResult;

        var entity = _mapper.Map<Domain.Entities.GroupNotice>(request);
        entity.Group = group;
        entity.AuthorId = profile.Id;

        await _commandContext.GroupNotices.AddAsync(entity);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the group notice with values {@Request}.", request);
            return new InvalidDatabaseOperationError("group");
        }

        var classTeacher = await _commandContext.SchoolProfiles.FindAsync(group.ClassTeacherId);
        var students = await _commandContext.SchoolProfiles
            .Where(s => s.Type == SchoolProfileType.Student && s.GroupId == group.Id)
            .ToListAsync();

        var parents = await _commandContext.SchoolProfiles
            .Include(p => p.Children)
            .Where(p => p.Type == SchoolProfileType.Parent)
            .ToListAsync(cancellationToken);

        var filteredParents = parents
            .Where(p => p.Children != null &&
                p.Children.Any(c => c.Type == SchoolProfileType.Student && c.GroupId == group.Id))
            .Distinct()
            .ToList();

        var combinedProfiles = new List<Domain.Entities.SchoolProfile>();
        if (classTeacher != null)
        {
            combinedProfiles.Add(classTeacher);
        }

        combinedProfiles.AddRange(filteredParents);
        combinedProfiles.AddRange(students);

        var clientUrl = _configuration["ClientUrl"]!;

        await SendEmailToCombinedProfiles(combinedProfiles, entity, group, clientUrl);

        var noticeResponse = _mapper.Map<GroupNoticeModelResponse>(entity);
        noticeResponse.Author = profile;
        return noticeResponse;
    }

    private async Task SendEmailToCombinedProfiles(List<Domain.Entities.SchoolProfile> combinedProfiles,
        Domain.Entities.GroupNotice entity,
        Domain.Entities.Group group,
        string clientUrl)
    {
        var tasks = combinedProfiles
            .Where(profile => profile.Email != null)
            .Select(async profile =>
            {
                try
                {
                    if (profile.Email != null)
                        await _mailService.SendAsync(
                            profile.Email,
                            EmailTemplates.NewGroupNotice.Subject,
                            EmailTemplates.NewGroupNotice.GetTemplate(entity.Title, group.Name, clientUrl, entity.Text));
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "An error occurred while send email to {@Email} after creating joining request.", profile.Email);
                }
            });

        await Task.WhenAll(tasks);
    }

    private async Task<Option<Error>> ValidateProfile(SchoolProfileType type, Guid userId, Domain.Entities.Group group)
    {
        switch (type)
        {
            case SchoolProfileType.SchoolAdmin or SchoolProfileType.Teacher:
                {
                    var validationError =
                        await _schoolProfileManager.ValidateSchoolProfileBySchool(userId, group.SchoolId);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.Student:
                {
                    var validationError = await _schoolProfileManager.ValidateSchoolProfileByGroup(userId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.ClassTeacher:
                {
                    var validationError = await _schoolProfileManager.ValidateClassTeacherSchoolProfileByGroup(userId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.Parent:
                {
                    var validationError = await _schoolProfileManager.ValidateParentProfileByChildGroup(userId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
        }

        return Option<Error>.None;
    }
}
