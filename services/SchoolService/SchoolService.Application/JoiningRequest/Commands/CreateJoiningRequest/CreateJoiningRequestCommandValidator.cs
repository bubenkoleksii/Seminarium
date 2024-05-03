namespace SchoolService.Application.JoiningRequest.Commands.CreateJoiningRequest;

public class CreateJoiningRequestCommandValidator : AbstractValidator<CreateJoiningRequestCommand>
{
    public CreateJoiningRequestCommandValidator()
    {
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

        RuleFor(x => x.RequesterEmail)
            .EmailAddress()
            .WithErrorCode(ErrorTitles.Common.Invalid)
            .MaximumLength(50)
            .WithErrorCode(ErrorTitles.Common.TooLong);

        RuleFor(x => x.RequesterPhone)
            .NotEmpty()
            .WithErrorCode(ErrorTitles.Common.Empty)
            .MaximumLength(50)
            .WithErrorCode(ErrorTitles.Common.TooLong);

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
