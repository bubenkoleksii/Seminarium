namespace CourseService.Application.Course.Queries.GetOneCourse;

public record GetOneCourseQuery(Guid Id, Guid UserId) : IRequest<Either<CourseModelResponse, Error>>;
