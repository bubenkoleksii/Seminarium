namespace SchoolService.Application.School.Queries.GetOneSchool;

public record GetOneSchoolQuery(Guid Id) : IRequest<Either<SchoolModelResponse, Error>>;
