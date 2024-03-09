using SchoolManagementService.Core.Domain.Errors;

namespace SchoolManagementService.Core.Application.School.Commands.DeleteSchool;

public record DeleteSchoolCommand(Guid Id) : IRequest<Option<Error>>;
