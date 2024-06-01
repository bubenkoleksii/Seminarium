namespace SchoolService.Application.School.Commands.CreateTeacherInvitation;

public class CreateTeacherInvitationCommandValidator : AbstractValidator<CreateTeacherInvitationCommand>
{
    public CreateTeacherInvitationCommandValidator()
    {
        RuleFor(x => x.SchoolId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
