using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Options;
using WebApplication_Benzeine.Options;

namespace WebApplication_Benzeine.Services
{
    public class EmailSender : IEmailSender
    {
        SendgridOptions sendgrid_options;

        public EmailSender()
            => sendgrid_options = new SendgridOptions();

        public Task<Response> SendEmailAsync(string email, string subject, string message)
        {
            return Execute(sendgrid_options.Key, subject, message, email);
        }


        private Task<Response> Execute(string key, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey: key);

            var _message = new SendGridMessage()
            {
                From = new EmailAddress("lotubala@cancan.com", name: "Jacky Moon"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            _message.AddTo(new EmailAddress(email));
            _message.SetClickTracking(false, false);

            return client.SendEmailAsync(_message);
        }







    }
}
