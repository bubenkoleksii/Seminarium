namespace Shared.Utils.Mail;

public class MailService(IOptions<MailOptions> options) : IMailService
{
    public async Task SendAsync(string to, string subject, string html)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Seminarium Administration", options.Value.User));
        email.To.Add(MailboxAddress.Parse(to));

        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(options.Value.Host, options.Value.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(options.Value.User, options.Value.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
