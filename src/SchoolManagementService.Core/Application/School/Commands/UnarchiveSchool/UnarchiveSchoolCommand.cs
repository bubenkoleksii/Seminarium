using SchoolManagementService.Core.Domain.Errors;

namespace SchoolManagementService.Core.Application.School.Commands.UnarchiveSchool;

public record UnarchiveSchoolCommand(Guid Id) : IRequest<Option<Error>>;
