namespace SchoolService.Application.JoiningRequest.Commands.CreateJoiningRequest;

public record CreateJoiningRequestCommand(
    ulong RegisterCode,

    string Name,

    string RequesterEmail,

    string RequesterPhone,

    string RequesterFullName,

    string? ShortName,

    uint GradingSystem,

    SchoolType Type,

    ulong PostalCode,

    SchoolOwnershipType OwnershipType,

    uint StudentsQuantity,

    SchoolRegion Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied = false
) : IRequest<Either<JoiningRequestModelResponse, Error>>;
