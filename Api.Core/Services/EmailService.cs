using Api.Core.Dtos.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Api.Core.Services
{
    public interface IEmailService
    {
        bool SendEmail(string to, string subject, string body, bool isBodyHtml = false, string from = null, string name = null);
    }
    public class EmailService: IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public bool SendEmail(string to, string subject, string body, bool isBodyHtml = false, string from = null, string name = null)
        {
            var message = new MailMessage();

            var toList = to.Split(new[] { ';' });

            foreach (var t in toList)
            {
                message.To.Add(t);
            }

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isBodyHtml;

            if (!string.IsNullOrEmpty(from))
            {
                message.From = new MailAddress(from, name);
            }
            else
            {
                message.From = new MailAddress(_appSettings.EmailFrom, name);
            }

            var client = new SmtpClient(_appSettings.SmtpHost, _appSettings.SmtpPort);
            client.Credentials = new System.Net.NetworkCredential(_appSettings.SmtpHost, _appSettings.SmtpPass);
            client.EnableSsl = true;

            client.Send(message);

            return true;
        }
    }
}

