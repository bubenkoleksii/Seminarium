namespace SchoolService.Application.SchoolProfile.Common.Invitation;

public interface IInvitationManager
{
    Either<InvitationSerializationData, Error> GetInvitationData(string invitationCode);
}
