using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PetShop.Infrastructure.Persistence;
using System.Net;
using System.Net.Mail;


namespace PetShop.Infrastructure.Services;

public class SmtpEmailSender : IEmailSender<ApplicationUser>
{
    private readonly IConfiguration _config;

    public SmtpEmailSender(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        => await SendAsync(email, "Redefinir senha - Cantinho da Roça",
            $"""
            <div style='font-family:sans-serif'>
                <h2>Redefinição de senha</h2>
                <p>Clique no botão abaixo para redefinir sua senha:</p>
                <a href='{resetLink}' style='background:#7c3aed;color:white;padding:12px 24px;
                   border-radius:8px;text-decoration:none'>Redefinir senha</a>
                <p style='color:#888;margin-top:16px'>Se não foi você, ignore este e-mail.</p>
            </div>
            """);

    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        => await SendAsync(email, "Confirme seu e-mail",
            $"<p>Clique aqui para confirmar: <a href='{confirmationLink}'>Confirmar e-mail</a></p>");

    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        => await SendAsync(email, "Código de redefinição",
            $"<p>Seu código: <strong>{resetCode}</strong></p>");

    private async Task SendAsync(string toEmail, string subject, string htmlBody)
    {
        var s = _config.GetSection("EmailSettings");

        using var client = new SmtpClient(s["SmtpHost"])
        {
            Port = int.Parse(s["SmtpPort"]!),
            Credentials = new NetworkCredential(s["SenderEmail"], s["Password"]),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(s["SenderEmail"]!, s["SenderName"]),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        mail.To.Add(toEmail);

        await client.SendMailAsync(mail);
    }
}