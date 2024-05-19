namespace SchoolService.Application.JoiningRequest.Queries.GetOneJoiningRequest;

public record GetOneJoiningRequestQuery(Guid Id) : IRequest<Either<JoiningRequestModelResponse, Error>>;
