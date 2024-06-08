namespace SchoolService.Domain.Entities;

public class StudyPeriod : Entity
{
    public Guid SchoolId { get; set; }

    public required School School { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}
