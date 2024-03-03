using System.ComponentModel;

namespace SchoolManagementService.Models.School;

public record CreateSchoolRequest(
    [Required][Range(1, ulong.MaxValue)] ulong RegisterCode,

    [Required][MaxLength(250)][MinLength(5)] string Name,

    [MaxLength(250)][MinLength(5)] string? ShortName,

    [Required][Range(1, uint.MaxValue)] uint GradingSystem,

    [EmailAddress][MaxLength(50)][MinLength(5)] string? Email,

    [Phone][MaxLength(50)][MinLength(5)] string? Phone,

    [Required][MaxLength(50)] string Type,

    [Required][Range(1, ulong.MaxValue)] ulong PostalCode,

    [Required][MaxLength(50)] string OwnershipType,

    [Required][Range(1, uint.MaxValue)] uint StudentsQuantity,

    [Required][MaxLength(50)] string Region,

    [MaxLength(250)][MinLength(5)] string? TerritorialCommunity,

    [MaxLength(250)][MinLength(5)] string? Address,

    [DefaultValue(false)] bool AreOccupied
);
