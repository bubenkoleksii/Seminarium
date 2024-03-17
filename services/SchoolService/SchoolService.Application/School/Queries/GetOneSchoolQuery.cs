namespace SchoolService.Application.School.Queries;

public record GetOneSchoolQuery(Guid Id) : IRequest<Either<SchoolModelResponse, Error>>;
