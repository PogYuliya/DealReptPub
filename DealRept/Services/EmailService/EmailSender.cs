using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DealRept.Services.EmailService
{
    public class EmailSender:IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

       
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Deal#Rept", _emailConfig.From));
            //emailMessage.To.AddRange(message.To);
            emailMessage.Bcc.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = message.IsMessageFromAdmin == true ? string.Format("<h3 style='color:#002d77;'>{0}</h3><div style='color:#002d77;'>Best regards, <br> <span style='color:#002d77;'>Deal</span><span style='color:#20c975;'>#</span><span style='color:#ff6825;'>Rept</span> Administrator</div>", message.Content)
                : string.Format("<p style='color:#002d77;'>{0}</p><div style='color:#002d77;'>Best regards, <br> <span style='color:#002d77;'>Deal</span><span style='color:#20c975;'>#</span><span style='color:#ff6825;'>Rept</span> User</div>", message.Content)
            };

            if (message.Attachments!=null&&message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms=new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client=new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message+ex.InnerException.Message);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }

            }
        }

        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch (Exception e)
                {
                    //log an error message or throw an exception, or both.
                    throw new Exception( e.Message);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
