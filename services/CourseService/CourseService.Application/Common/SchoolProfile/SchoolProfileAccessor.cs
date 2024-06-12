namespace CourseService.Application.Common.SchoolProfile;

public class SchoolProfileAccessor(IRequestClient<GetActiveSchoolProfileRequest> schoolProfileClient) : ISchoolProfileAccessor
{
    private readonly IRequestClient<GetActiveSchoolProfileRequest> _schoolProfileClient = schoolProfileClient;

    public async Task<Either<SchoolProfileContract, Error>> GetActiveSchoolProfile(
        GetActiveSchoolProfileRequest request,
        CancellationToken cancellationToken)
    {
        var getActiveSchoolProfileRequest = new GetActiveSchoolProfileRequest(
            request.UserId,
            [Constants.ClassTeacher, Constants.SchoolAdmin, Constants.Teacher]
        );

        try
        {
            var response =
                await _schoolProfileClient.GetResponse<GetActiveSchoolProfileResponse>(getActiveSchoolProfileRequest, cancellationToken);

            if (response.Message.Error == null && response.Message.SchoolProfile == null)
                return new InternalServicesError("school");

            if (response.Message.HasError && response.Message.Error != null)
                return response.Message.Error;

            var activeProfile = response.Message.SchoolProfile;
            if (activeProfile == null)
                return new InvalidError("school_profile");

            return activeProfile;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while sending get active school porfile with values @Request", getActiveSchoolProfileRequest);
            return new InternalServicesError("school");
        }
    }
}
