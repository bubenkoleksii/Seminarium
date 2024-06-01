namespace SchoolService.Application.Group.Commands.CreateStudentInvitation;

public class CreateStudentInvitationCommandValidator : AbstractValidator<CreateStudentInvitationCommand>
{
    public CreateStudentInvitationCommandValidator()
    {
        RuleFor(x => x.GroupId)
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
