namespace SchoolService.Application.Common.Options;

public class RabbitMqOptions
{
    public string Host { get; init; } = string.Empty;

    public string QueueName { get; init; } = string.Empty;

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
