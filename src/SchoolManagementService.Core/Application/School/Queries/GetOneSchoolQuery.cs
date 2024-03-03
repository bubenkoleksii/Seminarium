using SchoolManagementService.Core.Application.School.Models;
using SchoolManagementService.Core.Domain.Errors;

namespace SchoolManagementService.Core.Application.School.Queries;

public record GetOneSchoolQuery(Guid Id) : IRequest<Either<SchoolModelResponse, Error>>;
