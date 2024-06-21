namespace CourseService.Application.Common.DataContext;

public interface IContext
{
    DbSet<Domain.Entities.Course> Courses { get; set; }

    DbSet<Domain.Entities.CourseTeacher> CourseTeachers { get; set; }

    DbSet<Domain.Entities.CourseGroup> CourseGroups { get; set; }

    public DbSet<Domain.Entities.Lesson> Lessons { get; set; }

    public DbSet<Domain.Entities.Attachment> Attachments { get; set; }

    public DbSet<Domain.Entities.LessonItem> LessonItems { get; set; }

    public DbSet<Domain.Entities.TheoryLessonItem> TheoryLessonItems { get; set; }

    public DbSet<Domain.Entities.PracticalLessonItem> PracticalLessonItems { get; set; }

    public DbSet<Domain.Entities.PracticalLessonItemSubmit> PracticalLessonItemSubmits { get; set; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
