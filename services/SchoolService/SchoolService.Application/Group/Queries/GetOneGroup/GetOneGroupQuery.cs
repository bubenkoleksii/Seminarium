namespace SchoolService.Application.Group.Queries.GetOneGroup;

public record GetOneGroupQuery(
    Guid Id,

    Guid UserId
) : IRequest<Either<OneGroupModelResponse, Error>>;
