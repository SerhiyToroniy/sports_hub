using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using sports_hub.Services.Interfaces;

namespace sports_hub.Services.Implementations
{
    public class EmailService: IEmailService
    {
        public async Task SendConfirmationEmailAsync(string email, string callbackUrl, string language)
        {
            string subject = Constants.ConfirmEmailSubject;
            string filename, date;

            if (language == Constants.EnCookieName)
            {
                filename = Constants.EnConfirmEmailFilePath;
                date = Constants.EnglishDateTime;
            }
            else
            {
                filename = Constants.UkConfirmEmailFilePath;
                date = Constants.UkrainianDateTime;
            }

            await SendEmailFromFileAsync(email, subject, filename, date, callbackUrl);
        
        }

        public async Task SendEmailPasswordResetAsync(string email, string callbackUrl, string language)
        {
            string subject = Constants.ResetPasswordEmailSubject;
            string filename, date;

            if (language == Constants.EnCookieName)
            {
                filename = Constants.EnResetPasswordEmailFilePath;
                date = Constants.EnglishDateTime;
            }
            else
            {
                filename = Constants.UkResetPasswordEmailFilePath;
                date = Constants.UkrainianDateTime;
            }

            await SendEmailFromFileAsync(email, subject, filename, date, callbackUrl);

        }

        private async Task SendEmailFromFileAsync(string email, string subject, string fileName, params string[] parameters)
        {

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Sports Hub", Constants.SmtpServerEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            StreamReader streamReader = new(Directory.GetCurrentDirectory() + fileName);
            string html_template = streamReader.ReadToEnd();

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(html_template, parameters)
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Constants.SmtpServerAddress, Constants.SmtpServerPort, true);
                await client.AuthenticateAsync(Constants.SmtpServerEmail, Constants.SmtpServerPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
