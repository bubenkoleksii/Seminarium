using SchoolManagementService.Core.Application.School.Models;
using SchoolManagementService.Core.Domain.Enums.School;
using SchoolManagementService.Core.Domain.Errors;

namespace SchoolManagementService.Core.Application.School.Commands.CreateSchool;

public record CreateSchoolCommand(ulong RegisterCode,
    string Name,

    string? ShortName,

    uint GradingSystem,

    string? Email,

    string? Phone,

    SchoolType Type,

    ulong PostalCode,

    SchoolOwnershipType OwnershipType,

    uint StudentsQuantity,

    SchoolRegion Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied = false
) : IRequest<Either<SchoolModelResponse, Error>>;
