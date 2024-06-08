namespace SchoolService.Application.StudyPeriod.Commands.UpdateStudyPeriod;

public class UpdateStudyPeriodCommand : IRequest<Either<StudyPeriodModelResponse, Error>>
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}
