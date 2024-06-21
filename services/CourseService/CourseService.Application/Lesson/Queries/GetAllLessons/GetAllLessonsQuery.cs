namespace CourseService.Application.Lesson.Queries.GetAllLessons;

public class GetAllLessonsQuery : IRequest<Either<GetAllLessonsModelResponse, Error>>
{
    public Guid? CourseId { get; set; }

    public string? Topic { get; set; }
}
