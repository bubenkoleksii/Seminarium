namespace S3Service.Api.Consumers;

public class CreateFileConsumer : IConsumer<CreateFileContract>
{
    public async Task Consume(ConsumeContext<CreateFileContract> context)
    {
        Log.Information("----> Succes consuming" + context.Message.Name);
    }
}
