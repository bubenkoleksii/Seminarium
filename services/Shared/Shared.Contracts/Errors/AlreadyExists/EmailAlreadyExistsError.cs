namespace Shared.Contracts.Errors.AlreadyExists
{
    public class EmailAlreadyExistsError(string email, string resourceName = "resource") : AlreadyExistsError
    {
        public override string Detail { get; set; } =
            $"The {resourceName} with email address '{email}' already exists.";

        public override string Title => ErrorTitles.Common.EmailAlreadyExists;

        public override string Type => ErrorTypes.AlreadyExists;

        public override List<string> Params { get; set; } = new() { "email" };
    }
}
