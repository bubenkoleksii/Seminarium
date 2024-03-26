namespace S3Service.Api.Consumers;

public class SaveFileConsumer : IConsumer<SaveFileCommand>
{
    public async Task Consume(ConsumeContext<SaveFileCommand> context)
    {
        Log.Information("A file received: {@File}", context.Message);

        await context.RespondAsync(new SaveFileSuccess(Url: "asss", Name: "some"));
    }
}
