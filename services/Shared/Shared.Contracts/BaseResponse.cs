namespace Shared.Contracts;

public abstract class BaseResponse
{
    public Errors.Error? Error { get; set; }

    public bool HasError => Error != null;
}
