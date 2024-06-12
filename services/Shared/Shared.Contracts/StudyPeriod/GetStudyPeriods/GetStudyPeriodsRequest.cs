namespace Shared.Contracts.StudyPeriod.GetStudyPeriods;

public record GetStudyPeriodsRequest(
    Guid[]? Ids,

    Guid? SchoolId
);
