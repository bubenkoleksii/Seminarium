namespace SchoolService.Application.Group.Commands.CreateClassTeacherInvitation;

public class CreateClassTeacherInvitationCommandValidator : AbstractValidator<CreateClassTeacherInvitationCommand>
{
    public CreateClassTeacherInvitationCommandValidator()
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
