namespace Shared.Contracts.Errors.Invalid
{
    public class InvalidDatabaseOperationError(string resourceName = "resource", string operation = "") : Error
    {
        public override string Detail { get; set; } =
            $"The {operation} database operation with {resourceName} has been invalid.";

        public override string Type => ErrorTypes.Internal;
    }
}
