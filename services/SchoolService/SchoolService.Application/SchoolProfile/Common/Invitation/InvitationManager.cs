namespace SchoolService.Application.SchoolProfile.Common.Invitation;

public class InvitationManager : IInvitationManager
{
    private readonly IAesCipher _aesCipher;

    public InvitationManager(IAesCipher aesCipher)
    {
        _aesCipher = aesCipher;
    }

    public Either<Models.Invitation, Error> GetInvitationData(string invitationCode)
    {
        try
        {
            var decryptedJson = _aesCipher.Decrypt(invitationCode);
            var data = JsonConvert.DeserializeObject<Models.Invitation>(decryptedJson);

            if (data == null)
                return new InvalidError("invitation_code");

            return data;
        }
        catch
        {
            return new InvalidError("invitation_code");
        }
    }

    public string GenerateInvitationCode(Models.Invitation data)
    {
        var serializedJson = JsonConvert.SerializeObject(data);

        var encrypted = _aesCipher.Encrypt(serializedJson);
        return encrypted;
    }
}
