namespace SchoolService.Application.StudyPeriod.Models;

public record StudyPeriodModelResponse(
    Guid Id,

    Guid SchoolId,

    SchoolModelResponse School,

    DateOnly StartDate,

    DateOnly EndDate
);
