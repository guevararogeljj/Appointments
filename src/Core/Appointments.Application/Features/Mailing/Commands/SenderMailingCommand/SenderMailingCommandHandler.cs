using Appointments.Domain.Common;
using MediatR;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Appointments.Domain.Common;
using MailKit;
using Microsoft.Extensions.Configuration;

namespace Appointments.Application.Features.Mailing.Commands;

public class SenderMailingCommandHandler : IRequestHandler<SenderMailingCommand, Response<bool>>
{
    private readonly IConfiguration _configuration;
    public SenderMailingCommandHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<Response<bool>> Handle(SenderMailingCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<bool>();
        try
        {
            
            // Configuración para Gmail
            var smtpServer = _configuration["EmailSettings:SmtpServer"]; 
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]!); 
            var fromAddress = _configuration["EmailSettings:Username"];
            var smtpPassword = _configuration["EmailSettings:Password"];

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(fromAddress));
            email.To.Add(MailboxAddress.Parse(request.Email));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(fromAddress, smtpPassword, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
            response.Result = true;

        }
        catch (Exception ex)
        {
            response.Error = new Error(code: "MailingError", message: $"Error al enviar el correo: {ex.Message}");
            return response;
        }
        return response;
    }
}
