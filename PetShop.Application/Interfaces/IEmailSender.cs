namespace PetShop.Application.Interfaces;

public interface IAppEmailSender
{
    Task SendPasswordResetAsync(string toEmail, string resetLink);
    Task SendConfirmationAsync(string toEmail, string confirmationLink);
}