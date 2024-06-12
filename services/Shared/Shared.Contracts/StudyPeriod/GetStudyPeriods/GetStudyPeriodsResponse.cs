using LanguageExt;

namespace Shared.Contracts.StudyPeriod.GetStudyPeriods;

public class GetStudyPeriodsResponse
{
    public Either<IEnumerable<StudyPeriodContact>, Errors.Error> Result { get; set; }
}
