namespace Shared.Contracts.Errors.Services;

public class InternalServicesError(string service = "") : Error
{
    public override string Detail { get; set; } =
    $"The operation with {service} has been failed.";

    public override string Type => ErrorTypes.Internal;
}
