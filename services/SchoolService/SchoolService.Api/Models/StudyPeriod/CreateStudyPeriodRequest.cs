namespace SchoolService.Api.Models.StudyPeriod;

public record CreateStudyPeriodRequest(
    DateOnly StartDate,

    DateOnly EndDate
);
