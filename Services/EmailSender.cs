using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

using System.Net.Mail;

namespace Badge.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger, IConfiguration configuration)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
        _configuration = configuration;
    }

    public AuthMessageSenderOptions Options { get; } 

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute(subject, message, toEmail);
    }

    public async Task Execute(string subject, string message, string toEmail)
    {
        var authEmail = Options.AuthEmail;
        var authPassword = Options.AuthEmailPassword;
        SmtpClient smtpClient = new SmtpClient("websmtp.simply.com", 587);
        smtpClient.Credentials = new System.Net.NetworkCredential(authEmail, authPassword);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;
        
        
        // Opretter email

        MailMessage msg = new MailMessage()
        {
            From = new MailAddress("Badge@civah.dk", "Badge"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true

        };

        // Sender email
        msg.To.Add(toEmail);
        await smtpClient.SendMailAsync(msg);
    }
}