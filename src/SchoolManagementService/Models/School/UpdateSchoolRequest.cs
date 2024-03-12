using SchoolManagementService.Core.Application.Common.CloudStorage;
using SchoolManagementService.Files;

namespace SchoolManagementService.Models.School;

public record UpdateSchoolRequest(
    Guid Id,

    ulong RegisterCode,

    string Name,

    string? ShortName,

    uint GradingSystem,

    string? Email,

    string? Phone,

    string Type,

    ulong PostalCode,

    string OwnershipType,

    uint StudentsQuantity,

    string Region,

    string? TerritorialCommunity,

    string? Address,

    bool AreOccupied,

    string? SiteUrl,

    [FileValidation(FileKind.Image, 2)] IFormFile? Img
);
