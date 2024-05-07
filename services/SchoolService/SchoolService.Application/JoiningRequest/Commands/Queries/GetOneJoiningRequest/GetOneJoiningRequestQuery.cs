namespace SchoolService.Application.JoiningRequest.Commands.Queries.GetOneJoiningRequest;

public record GetOneJoiningRequestQuery(Guid Id) : IRequest<Either<JoiningRequestModelResponse, Error>>;
