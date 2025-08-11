using Appointments.Domain.Common;
using MediatR;
using Microsoft.Extensions.Configuration;
using Appointments.Domain.Common;
using MediatR;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit;
namespace Appointments.Application.Features.Mailing.Commands.SenderMailingsCommand;

public class SenderMailingsCommandHandler : IRequestHandler<SenderMailingsCommand, Response<bool>>
{
    private readonly IConfiguration _configuration;
    public SenderMailingsCommandHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<Response<bool>> Handle(SenderMailingsCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<bool>();
        try
        {
            // Configuración para el servidor SMTP
            var smtpServer = _configuration["EmailSettings:SmtpServer"]; 
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]!); 
            var fromAddress = _configuration["EmailSettings:Username"];
            var smtpPassword = _configuration["EmailSettings:Password"];

            foreach (var mail in request.Mailing!)
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(fromAddress));
                email.To.Add(MailboxAddress.Parse(mail.Email));
                email.Subject = mail.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = mail.Body };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls, cancellationToken);
                await smtp.AuthenticateAsync(fromAddress, smtpPassword, cancellationToken);
                await smtp.SendAsync(email, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);
            }
            
            response.Result = true;
        }
        catch (Exception ex)
        {
            response.Error = new Error(
                "SenderMailingsCommandError",
                $"An error occurred while sending mailings: {ex.Message}"
            );
        }

        return response;
    }
}