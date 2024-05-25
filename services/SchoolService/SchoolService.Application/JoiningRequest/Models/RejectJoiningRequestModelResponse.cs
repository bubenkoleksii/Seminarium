namespace SchoolService.Application.JoiningRequest.Models;

public record RejectJoiningRequestModelResponse(
    Guid Id,

    JoiningRequestStatus Status
);
