namespace Shared.Contracts.Errors.Invalid;

public class InvalidFileOperationError(string name = "resource", string operation = "") : Error
{
    public override string Detail { get; set; } =
        $"The {operation} operation with {name} has been invalid.";

    public override string Type => ErrorTypes.Invalid;
}
