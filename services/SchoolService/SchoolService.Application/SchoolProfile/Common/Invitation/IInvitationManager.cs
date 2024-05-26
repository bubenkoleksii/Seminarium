namespace SchoolService.Application.SchoolProfile.Common.Invitation;

public interface IInvitationManager
{
    Either<Models.Invitation, Error> GetInvitationData(string invitationCode);

    string GenerateInvitationCode(Models.Invitation data);
}
