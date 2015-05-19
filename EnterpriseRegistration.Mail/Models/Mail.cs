using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Mail.Models
{
    public class Mail
    {
        public String From { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
        public DateTime ReceivedDate { get; set; }
        public List<Attachment> Attachments { get; set; }

        public static explicit operator Mail(OpenPop.Mime.Message msg)
        {
            
            Mail mail = new Mail();
            mail.From = msg.Headers.From.Address;
            mail.Subject = msg.Headers.Subject;
            mail.ReceivedDate = msg.Headers.DateSent;
            try
            {
                mail.Body = msg.FindFirstHtmlVersion().GetBodyAsText();
            }
            catch
            {
                mail.Body = System.Text.Encoding.Default.GetString(msg.FindFirstPlainTextVersion().Body);
                //throw ex;
            }
            mail.Attachments = new List<Attachment>();
            foreach(var attachment in msg.FindAllAttachments())
            {
                Attachment atmt = new Attachment();
                atmt.Name = attachment.FileName;
                atmt.Content = attachment.Body;
                atmt.MIME = attachment.ContentType.MediaType;
                mail.Attachments.Add(atmt);
            }

            return mail;
        }
    }
}
