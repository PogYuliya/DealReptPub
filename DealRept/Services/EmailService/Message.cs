using Microsoft.AspNetCore.Http;
using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace DealRept.Services.EmailService
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IFormFileCollection Attachments { get; set; }
        public bool IsMessageFromAdmin { get; set; }

        public Message(IEnumerable<string> to, string subject,
             string content, IFormFileCollection attachments, bool isMessageFromAdmin)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress("user",x)));

            Subject = subject;
            Content = content;
            Attachments = attachments;
            IsMessageFromAdmin = isMessageFromAdmin;
        }
    }
}
