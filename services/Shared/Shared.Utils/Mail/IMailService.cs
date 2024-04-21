namespace Shared.Utils.Mail;

public interface IMailService
{
    public Task SendAsync(string to, string subject, string html);
}
