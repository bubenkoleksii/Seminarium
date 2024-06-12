namespace Shared.Contracts.StudyPeriod;

public class StudyPeriodContact
{
    public Guid Id { get; set; }

    public Guid SchoolId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}
