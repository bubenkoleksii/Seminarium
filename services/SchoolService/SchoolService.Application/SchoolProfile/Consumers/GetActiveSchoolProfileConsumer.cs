namespace SchoolService.Application.SchoolProfile.Consumers;

public class GetActiveProfileConsumer(ISchoolProfileManager schoolProfileManager, IMapper mapper) : IConsumer<GetActiveSchoolProfileRequest>
{
    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly IMapper _mapper = mapper;

    public async Task Consume(ConsumeContext<GetActiveSchoolProfileRequest> context)
    {
        Either<SchoolProfileContract, Error> response;

        var activeProfile = await _schoolProfileManager.GetActiveProfile(context.Message.UserId);
        if (activeProfile == null)
        {
            response = new NotFoundByIdError(context.Message.UserId, "school_profile");

            await context.RespondAsync(response);
            return;
        }

        var schoolProfileResponse = _mapper.Map<SchoolProfileContract>(activeProfile);

        var isInvalidSchoolProfileType = !IsValidSchoolProfileType(
            context.Message.AllowedProfileTypes,
            activeProfile.Type.ToString().ToSnakeCase());

        if (isInvalidSchoolProfileType)
        {
            response = new InvalidError("school_profile");

            await context.RespondAsync(response);
            return;
        }

        response = schoolProfileResponse;
        await context.RespondAsync(response);
    }

    private static bool IsValidSchoolProfileType(string[] allowedProfiles, string profile) =>
        allowedProfiles.Exists(s => s.Contains(profile));
}
