namespace SchoolService.Api.Models.StudyPeriod;

public record UpdateStudyPeriodRequest(
    Guid Id,

    DateOnly StartDate,

    DateOnly EndDate
);
