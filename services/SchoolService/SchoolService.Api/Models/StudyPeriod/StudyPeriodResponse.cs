namespace SchoolService.Api.Models.StudyPeriod;

public record StudyPeriodResponse(
    Guid Id,

    Guid SchoolId,

    SchoolResponse School,

    DateOnly StartDate,

    DateOnly EndDate
);
