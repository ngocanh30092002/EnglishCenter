using EnglishCenter.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EnglishCenter.Presentation.Helpers
{
    public class MailHelper
    {
        private readonly MailSetting _mailSetting;

        public MailHelper(IOptions<MailSetting> mailSetting)
        {
            _mailSetting = mailSetting.Value;
        }
        public async Task<bool> SendHtmlMailAsync(MailContent content)
        {
            bool isSuccess = true;
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail);
            email.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail));
            email.To.Add(new MailboxAddress(content.To, content.To));
            email.Subject = content.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = content.Body;
            email.Body = builder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(_mailSetting.Host, _mailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_mailSetting.Mail, _mailSetting.Password);
                    await smtp.SendAsync(email);

                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
                finally
                {
                    await smtp.DisconnectAsync(true);
                }
            }

            return isSuccess;
        }

        public async Task<bool> SendMailAsync(MailContent content)
        {
            bool isSuccess = true;
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail);
            email.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail));
            email.To.Add(new MailboxAddress(content.To, content.To));
            email.Subject = content.Subject;

            var builder = new BodyBuilder();
            builder.TextBody = content.Body;
            email.Body = builder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(_mailSetting.Host, _mailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_mailSetting.Mail, _mailSetting.Password);
                    await smtp.SendAsync(email);

                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
                finally
                {
                    await smtp.DisconnectAsync(true);
                }
            }

            return isSuccess;
        }
    }
}
