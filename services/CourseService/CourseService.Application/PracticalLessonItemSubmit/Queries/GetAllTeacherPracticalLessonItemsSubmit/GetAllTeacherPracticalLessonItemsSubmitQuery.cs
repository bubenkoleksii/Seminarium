namespace CourseService.Application.PracticalLessonItemSubmit.Queries.GetAllTeacherPracticalLessonItemsSubmit;

public record GetAllTeacherPracticalLessonItemsSubmitQuery(Guid ItemId, uint Skip, uint? Take)
    : IRequest<Either<GetAllPracticalLessonItemSubmitModelResponse, Error>>;
