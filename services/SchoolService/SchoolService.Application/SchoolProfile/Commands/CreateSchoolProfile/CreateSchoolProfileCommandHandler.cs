using Newtonsoft.Json;

namespace SchoolService.Application.SchoolProfile.Commands.CreateSchoolProfile;

public class CreateSchoolProfileCommandHandler : IRequestHandler<CreateSchoolProfileCommand, Either<SchoolProfileModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IAesCipher _aesCipher;

    public CreateSchoolProfileCommandHandler(ICommandContext commandContext, IAesCipher aesCipher)
    {
        _commandContext = commandContext;
        _aesCipher = aesCipher;
    }

    public async Task<Either<SchoolProfileModelResponse, Error>> Handle(CreateSchoolProfileCommand request, CancellationToken cancellationToken)
    {
        await _commandContext.SchoolProfiles.ToListAsync();

        var invitationData = GetInvitationData(request.InvitationCode);
        if (invitationData.IsRight)
        {
            Log.Error("An error occurred while reading invitation code with values {@InvitationCode}.", request.InvitationCode);
            return (Error)invitationData;
        }

        var schoolProfileResponse = new SchoolProfileModelResponse(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, null, request.InvitationCode, null, null, null, null, null);
        return schoolProfileResponse;
    }

    private Either<InvitationSerializationData, Error> GetInvitationData(string invitationCode)
    {
        try
        {
            var decryptedJson = _aesCipher.Decrypt(invitationCode);
            var data = JsonConvert.DeserializeObject<InvitationSerializationData>(decryptedJson);

            if (data == null)
                return new InvalidError("invitation_code");

            return data;
        }
        catch
        {
            return new InvalidError("invitation_code");
        }
    }
}
