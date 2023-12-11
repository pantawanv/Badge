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

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute(subject, message, toEmail);
    }

    public async Task Execute(string subject, string message, string toEmail)
    {
        //var client = new SendGridClient(apiKey);
        SmtpClient smtpClient = new SmtpClient("smtp.simply.com", 587);
        string email = _configuration["EmailSender:Email"];
        smtpClient.Credentials = new System.Net.NetworkCredential("badge@civah.dk", "Password123.");
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;

        MailMessage msg = new MailMessage()
        {
            From = new MailAddress("Badge@civah.dk", "Badge"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true

        };
        msg.To.Add(toEmail);
        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html


        await smtpClient.SendMailAsync(msg);

        //msg.SetClickTracking(false, false);
        //var response = await client.SendEmailAsync(msg);
        //_logger.LogInformation(response.IsSuccessStatusCode
        //? $"Email to {toEmail} queued successfully!"
        //: $"Failure Email to {toEmail}");
    }
}