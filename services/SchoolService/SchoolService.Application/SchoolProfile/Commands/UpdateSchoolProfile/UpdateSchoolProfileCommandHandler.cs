namespace SchoolService.Application.SchoolProfile.Commands.UpdateSchoolProfile;

public class UpdateSchoolProfileCommandHandler : IRequestHandler<UpdateSchoolProfileCommand, Either<SchoolProfileModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IMapper _mapper;

    public UpdateSchoolProfileCommandHandler(
        ICommandContext commandContext,
        ISchoolProfileManager schoolProfileManager,
        IMapper mapper)
    {
        _commandContext = commandContext;
        _schoolProfileManager = schoolProfileManager;
        _mapper = mapper;
    }

    public async Task<Either<SchoolProfileModelResponse, Error>> Handle(UpdateSchoolProfileCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.SchoolProfiles
            .IgnoreQueryFilters()
            .Include(profile => profile.School)
            .Include(profile => profile.Group)
            .Include(profile => profile.ClassTeacherGroup)
            .Include(profile => profile.Children)
            .Include(profile => profile.Parents)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null || entity.UserId != request.UserId)
            return new NotFoundByIdError(request.Id, "school_profile");

        var activeProfile = entity.IsActive;

        _mapper.Map(request, entity);
        entity.IsActive = activeProfile;

        string? serializationData = null;
        switch (entity.Type)
        {
            case SchoolProfileType.Teacher:
                if (request.TeacherLessonsPerCycle is null)
                    return new InvalidError("lessons_per_cycle");

                var teacherData = new TeacherSerializationData(
                    request.TeacherSubjects,
                    request.TeacherExperience,
                    request.TeacherEducation,
                    request.TeacherQualification,
                    (uint)request.TeacherLessonsPerCycle
                );

                serializationData = JsonConvert.SerializeObject(teacherData);
                break;
            case SchoolProfileType.Student:
                if (request.StudentIsIndividually is null)
                    return new InvalidError("is_individually");

                if (request.StudentIsClassLeader is null)
                    return new InvalidError("is_class_leader");

                if (request.StudentHealthGroup is null)
                    return new InvalidError("health_group");

                var studentData = new StudentSerializationData(
                    request.StudentDateOfBirth,
                    request.StudentAptitudes,
                    (bool)request.StudentIsClassLeader,
                    (bool)request.StudentIsIndividually,
                    (HealthGroup)request.StudentHealthGroup
                );

                serializationData = JsonConvert.SerializeObject(studentData);

                break;
            case SchoolProfileType.Parent:
                serializationData = JsonConvert.SerializeObject(
                    new ParentSerializationData(request.ParentAddress)
                );
                break;
        }

        entity.Data = serializationData;
        _commandContext.SchoolProfiles.Update(entity);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while updating the school with values {@Request}.", request);

            return new InvalidDatabaseOperationError("school");
        }

        _schoolProfileManager.ClearCache(request.UserId);

        var schoolProfileModelResponse = _mapper.Map<SchoolProfileModelResponse>(entity);
        return schoolProfileModelResponse;
    }
}
