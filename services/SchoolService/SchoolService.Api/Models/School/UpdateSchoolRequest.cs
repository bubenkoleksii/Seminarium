﻿namespace SchoolService.Api.Models.School;

public record UpdateSchoolRequest(Guid Id,

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

    string? SiteUrl
);
