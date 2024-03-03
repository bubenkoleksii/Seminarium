namespace SchoolManagementService.Core.Application.School.Commands.CreateSchool;

public class CreateSchoolCommandValidator : AbstractValidator<CreateSchoolCommand>
{
    public CreateSchoolCommandValidator()
    {
        RuleFor(x => x.RegisterCode)
            .NotEmpty()
            .GreaterThan((ulong)0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(5, 250);

        RuleFor(x => x.ShortName)
            .Length(5, 250);

        RuleFor(x => x.GradingSystem)
            .NotEmpty()
            .GreaterThan((uint)0);

        RuleFor(x => x.Email)
            .EmailAddress()
            .Length(5, 50);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Length(5, 50);

        RuleFor(x => x.Type)
            .NotEmpty();

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .GreaterThan((ulong)0);

        RuleFor(x => x.OwnershipType)
            .NotEmpty();

        RuleFor(x => x.StudentsQuantity)
            .NotEmpty()
            .GreaterThan((uint)0);

        RuleFor(x => x.Region)
            .NotEmpty();

        RuleFor(x => x.TerritorialCommunity)
            .Length(5, 250);

        RuleFor(x => x.Address)
            .Length(5, 250);
    }
}
