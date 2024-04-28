namespace SchoolService.Application.School.Commands.CreateSchool;

public class CreateSchoolCommandValidator : AbstractValidator<CreateSchoolCommand>
{
    public CreateSchoolCommandValidator()
    {
        RuleFor(x => x.JoiningRequestId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.RegisterCode)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty)
            .GreaterThan((ulong)0)
            .WithErrorCode(ErrorTitles.Common.LowerZero);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty)
            .MaximumLength(250)
            .WithErrorCode(ErrorTitles.Common.TooLong);

        RuleFor(x => x.ShortName)
            .MaximumLength(250)
            .WithErrorCode(ErrorTitles.Common.TooLong);

        RuleFor(x => x.GradingSystem)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty)
            .GreaterThan((uint)0)
            .WithErrorCode(ErrorTitles.Common.LowerZero);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty)
            .GreaterThan((ulong)0)
            .WithErrorCode(ErrorTitles.Common.LowerZero);

        RuleFor(x => x.OwnershipType)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.StudentsQuantity)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty)
            .GreaterThan((uint)0)
            .WithErrorCode(ErrorTitles.Common.LowerZero);

        RuleFor(x => x.Region)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.TerritorialCommunity)
            .MaximumLength(250)
            .WithErrorCode(ErrorTitles.Common.TooLong);

        RuleFor(x => x.Address)
            .MaximumLength(250)
            .WithErrorCode(ErrorTitles.Common.TooLong);
    }
}
