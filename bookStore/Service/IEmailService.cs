using System.Threading.Tasks;
using bookStore.Models;

namespace bookStore.Service
{
    public interface IEmailService 
    {
        Task SendTestEmail(UserEmailModel userEmailModel);
        Task SendEmailForEmailConfirmation(UserEmailModel userEmailModel);
        // Task SendEmail(UserEmailModel userEmailModel);
        // string GetEmailBody(string templateName);

        Task ForgotPassword(UserEmailModel userEmailModel);
    }
}