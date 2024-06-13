namespace CourseService.Infrastructure.Persistence;

public abstract class BaseContext(DbContextOptions options) : DbContext(options)
{
    public const string Schema = "public";
    public const string MigrationsTableName = "__EFMigrationsHistory";

    public DbSet<Course> Courses { get; set; }

    public DbSet<CourseTeacher> CourseTeachers { get; set; }

    public DbSet<CourseGroup> CourseGroups { get; set; }

    public DbSet<Lesson> Lessons { get; set; }

    public DbSet<Attachment> Attachments { get; set; }

    public DbSet<LessonItem> LessonItems { get; set; }

    public DbSet<TheoryLessonItem> TheoryLessonItems { get; set; }

    public DbSet<PracticalLessonItem> PracticalLessonItems { get; set; }

    public DbSet<PracticalLessonItemSubmit> PracticalLessonItemSubmits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new CourseGroupConfiguration());
        modelBuilder.ApplyConfiguration(new CourseTeacherConfiguration());
        modelBuilder.ApplyConfiguration(new LessonConfiguration());
        modelBuilder.ApplyConfiguration(new AttachmentConfiguration());
        modelBuilder.ApplyConfiguration(new LessonItemConfiguration());
        modelBuilder.ApplyConfiguration(new PracticalLessonItemConfiguration());
        modelBuilder.ApplyConfiguration(new PracticalLessonItemSubmitConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
