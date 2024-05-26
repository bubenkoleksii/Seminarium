namespace SchoolService.Application.SchoolProfile.Common.Invitation;

public class InvitationManager : IInvitationManager
{
    private readonly IAesCipher _aesCipher;

    public InvitationManager(IAesCipher aesCipher)
    {
        _aesCipher = aesCipher;
    }

    public Either<InvitationSerializationData, Error> GetInvitationData(string invitationCode)
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
