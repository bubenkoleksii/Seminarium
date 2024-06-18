namespace CourseService.Application.Course.Consumers;

public class DeleteCoursesConsumer(ICommandContext commandContext) : IConsumer<DeleteCoursesRequest>
{
    private readonly ICommandContext _commandContext = commandContext;

    public async Task Consume(ConsumeContext<DeleteCoursesRequest> context)
    {
        var courses = await _commandContext.Courses
           .Where(c => c.StudyPeriodId == context.Message.StudyPeriodId)
           .ToListAsync();

        if (courses.Count != 0)
        {
            _commandContext.Courses.RemoveRange(courses);

            try
            {
                await _commandContext.SaveChangesAsync(cancellationToken: CancellationToken.None);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to delete courses with study period id @id", context.Message.StudyPeriodId);
            }
        }
    }
}
