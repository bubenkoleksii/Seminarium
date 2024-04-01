namespace SchoolService.Application.School.Commands.DeleteSchoolImage;

public class DeleteSchoolImageCommandValidator : AbstractValidator<DeleteSchoolImageCommand>
{
    public DeleteSchoolImageCommandValidator()
    {
        RuleFor(x => x.SchoolId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
