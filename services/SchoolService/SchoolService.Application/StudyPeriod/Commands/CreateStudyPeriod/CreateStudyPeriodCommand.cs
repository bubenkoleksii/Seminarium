namespace SchoolService.Application.StudyPeriod.Commands.CreateStudyPeriod;

public class CreateStudyPeriodCommand : IRequest<Either<StudyPeriodModelResponse, Error>>
{
    public Guid UserId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}
