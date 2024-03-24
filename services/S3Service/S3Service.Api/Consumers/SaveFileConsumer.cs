namespace S3Service.Api.Consumers;

public class SaveFileConsumer : IConsumer<SaveFile>
{
    public Task Consume(ConsumeContext<SaveFile> context)
    {
        Log.Information("A file received: {@File}", context.Message);
        return Task.CompletedTask;
    }
}
