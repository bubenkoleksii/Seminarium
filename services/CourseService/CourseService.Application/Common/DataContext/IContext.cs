namespace CourseService.Application.Common.DataContext;

public interface IContext
{
    DbSet<Domain.Entities.Course> Courses { get; set; }

    DbSet<Domain.Entities.CourseTeacher> CourseTeachers { get; set; }

    DbSet<Domain.Entities.CourseGroup> CourseGroups { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
