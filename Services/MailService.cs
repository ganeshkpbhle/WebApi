using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;
using WebApi.Models;
using WebApi.Settings;

namespace WebApi.Services
{
    public class MailService : IMail
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var msg = new MimeMessage();
            msg.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            msg.To.Add(MailboxAddress.Parse(mailRequest.ToMail));
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileByt;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileByt = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileByt, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            msg.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host,_mailSettings.Port,MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail,_mailSettings.Password);
            await smtp.SendAsync(msg);
            smtp.Disconnect(true);
        }
    }
}