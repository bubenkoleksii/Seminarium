namespace Shared.Contracts.StudyPeriod.GetStudyPeriods;

public class GetStudyPeriodsResponse : BaseResponse
{
    public IEnumerable<StudyPeriodContact>? StudyPeriods { get; set; }
}
