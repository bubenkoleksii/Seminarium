namespace SchoolService.Application.StudyPeriod.Commands.DeleteStudyPeriod;

public record DeleteStudyPeriodCommand(
    Guid Id,

    Guid UserId
) : IRequest<Option<Error>>;
