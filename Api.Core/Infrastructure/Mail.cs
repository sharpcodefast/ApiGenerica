using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Mail;

namespace Api.Core.Infrastructure
{
    public static class Mail
    {
        public static bool SendEmail(string to, string subject, string body, AppSettings _appSettings, bool isBodyHtml = false, string from = null, IList<CustomFileBase> actualFiles = null, string name = null)
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
            else {
                message.From = new MailAddress(_appSettings.EmailFrom, name);
            }
            //if (files != null)
            //{
            //    foreach (var file in files)
            //    {
            //        message.Attachments.Add(new Attachment(file.InputStream, file.FileName, file.ContentType));
            //    }
            //}

            if (actualFiles != null)
            {
                foreach (var file in actualFiles)
                {
                    message.Attachments.Add(new Attachment(file.Stream, file.FileName));
                }
            }

            var client = new SmtpClient(_appSettings.SmtpHost, _appSettings.SmtpPort);
            client.Credentials = new System.Net.NetworkCredential(_appSettings.SmtpUser, _appSettings.SmtpPass);
            client.EnableSsl = true;

            client.Send(message);

            return true;
        }
    }
}
