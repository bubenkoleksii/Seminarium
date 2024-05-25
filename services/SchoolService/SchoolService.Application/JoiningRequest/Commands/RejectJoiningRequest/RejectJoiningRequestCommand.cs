namespace SchoolService.Application.JoiningRequest.Commands.RejectJoiningRequest;

public record RejectJoiningRequestCommand(
    Guid Id,

    string? Text
) : IRequest<Either<RejectJoiningRequestModelResponse, Error>>;
