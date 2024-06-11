namespace SchoolService.Application.StudyPeriod.Models;

public class StudyPeriodModelResponse
{
    public Guid Id { get; set; }

    public Guid SchoolId { get; set; }

    public required SchoolModelResponse School { get; set; }

    public bool IncrementGroups { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}

