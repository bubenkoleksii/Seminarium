namespace CourseService.Infrastructure.Persistence;

public abstract class BaseContext(DbContextOptions options) : DbContext(options)
{
    public const string Schema = "public";
    public const string MigrationsTableName = "__EFMigrationsHistory";

    public DbSet<Course> Courses { get; set; }

    public DbSet<CourseTeacher> CourseTeachers { get; set; }

    public DbSet<CourseGroup> CourseGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new CourseGroupConfiguration());
        modelBuilder.ApplyConfiguration(new CourseTeacherConfiguration());
        modelBuilder.ApplyConfiguration(new LessonConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
