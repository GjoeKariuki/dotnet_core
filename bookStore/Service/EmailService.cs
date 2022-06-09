using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;


using Microsoft.Extensions.Options;

using bookStore.Models;



namespace bookStore.Service
{

    public class EmailService : IEmailService
    {

        private const string templatePath = @"EmailTemplate/{0}.html";
        
        private readonly SMTPConfigModel _smtpConfig;

        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public async Task SendTestEmail(UserEmailModel userEmailModel)
        {
            userEmailModel.Subject = UpdatePlaceHolders("hibajrnkkal{{UserName}}", userEmailModel.Placeholders);
            userEmailModel.Body = UpdatePlaceHolders(GetEmailBody("TestEmail"), userEmailModel.Placeholders);
            await SendEmail(userEmailModel);
        } 
        public async Task SendEmailForEmailConfirmation(UserEmailModel userEmailModel)
        {
            userEmailModel.Subject = UpdatePlaceHolders("hibajrnkkal{{UserName}} confirm email", userEmailModel.Placeholders);
            userEmailModel.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), userEmailModel.Placeholders);
            await SendEmail(userEmailModel);
        }

        public async Task ForgotPassword(UserEmailModel userEmailModel)
        {
            userEmailModel.Subject = UpdatePlaceHolders("{{UserName}} reset your password here", userEmailModel.Placeholders);
            userEmailModel.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailModel.Placeholders);
            await SendEmail(userEmailModel);
        }
        private async Task SendEmail(UserEmailModel userEmailModel)
        {
            MailMessage mail = new MailMessage
            {
                Subject = userEmailModel.Subject,
                Body = userEmailModel.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName)
            };
            foreach (var toEmail in userEmailModel.ToEmails)
            {
                mail.To.Add(toEmail);
            }
            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;
            await smtpClient.SendMailAsync(mail);
        }

        private string GetEmailBody(string templateName)
        {
            return File.ReadAllText(string.Format(templatePath, templateName));
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string,string>> keyValuePairs)
        {
            if(!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach(var placeholder in keyValuePairs){
                    if (text.Contains(placeholder.Key)){
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }
            return text;
        }
    }
}