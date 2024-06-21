namespace CourseService.Application.Course.Queries.GetAllCourses;

public class GetAllCoursesQuery : IRequest<Either<GetAllCoursesModelResponse, Error>>
{
    public Guid UserId { get; set; }

    public Guid? GroupId { get; set; }

    public Guid? TeacherId { get; set; }

    public Guid? StudyPeriodId { get; set; }

    public string? Name { get; set; }

    public uint Skip { get; set; }

    public uint? Take { get; set; }
}
