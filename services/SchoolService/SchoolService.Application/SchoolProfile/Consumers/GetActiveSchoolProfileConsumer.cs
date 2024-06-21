namespace SchoolService.Application.SchoolProfile.Consumers;

public class GetActiveProfileConsumer(ISchoolProfileManager schoolProfileManager, IMapper mapper) : IConsumer<GetActiveSchoolProfileRequest>
{
    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly IMapper _mapper = mapper;

    public async Task Consume(ConsumeContext<GetActiveSchoolProfileRequest> context)
    {
        var response = new GetActiveSchoolProfileResponse();

        var activeProfile = await _schoolProfileManager.GetActiveProfile(context.Message.UserId);
        if (activeProfile == null)
        {
            response.Error = new NotFoundByIdError(context.Message.UserId, "school_profile");

            await context.RespondAsync(response);
            return;
        }

        var schoolProfileResponse = _mapper.Map<SchoolProfileContract>(activeProfile);

        var isInvalidSchoolProfileType = !IsValidSchoolProfileType(
            context.Message.AllowedProfileTypes,
            activeProfile.Type.ToString().ToSnakeCase());

        if (isInvalidSchoolProfileType)
        {
            response.Error = new InvalidError("school_profile");

            await context.RespondAsync(response);
            return;
        }

        response.SchoolProfile = schoolProfileResponse;
        await context.RespondAsync(response);
    }

    private static bool IsValidSchoolProfileType(string[] allowedProfiles, string profile) =>
        allowedProfiles.Exists(s => s.Contains(profile));
}
