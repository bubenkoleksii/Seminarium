namespace CourseService.Application.LessonItem.Queries.PracticalLessonItem.GetAllStudentPracticalLessonItems;

public record GetAllStudentPracticalLessonItemsQuery(
    Guid UserId,

    Guid StudentId,

    uint Skip,

    uint? Take
) : IRequest<Either<GetAllStudentPracticalLessonItemsModelResponse, Error>>;
