namespace CourseService.Application.PracticalLessonItemSubmit.Queries.GetOnePracticalLessonItemSubmit;

public record GetOnePracticalLessonItemSubmitQuery(Guid ItemId, Guid StudentId)
    : IRequest<Either<PracticalLessonItemSubmitModelResponse?, Error>>;
