namespace SchoolService.Application.StudyPeriod.Queries.GetAllStudyPeriods;

public record GetAllStudyPeriodsQuery(
    Guid UserId
 ) : IRequest<IEnumerable<StudyPeriodModelResponse>>;
