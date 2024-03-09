using SchoolManagementService.Core.Domain.Errors;

namespace SchoolManagementService.Core.Application.School.Commands.ArchiveSchool;

public record ArchiveSchoolCommand(Guid Id) : IRequest<Option<Error>>;
