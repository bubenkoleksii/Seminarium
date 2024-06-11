namespace SchoolService.Api.Models.StudyPeriod;

public record CreateStudyPeriodRequest(
    bool IncrementGroups,

    DateOnly StartDate,

    DateOnly EndDate
);
