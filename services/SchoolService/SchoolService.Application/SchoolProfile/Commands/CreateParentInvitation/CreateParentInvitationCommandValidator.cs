namespace SchoolService.Application.SchoolProfile.Commands.CreateParentInvitation;

public class CreateParentInvitationCommandValidator : AbstractValidator<CreateParentInvitationCommand>
{
    public CreateParentInvitationCommandValidator()
    {
        RuleFor(x => x.ChildId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.UserId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
