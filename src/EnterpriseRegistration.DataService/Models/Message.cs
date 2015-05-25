using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.DataService.Models
{
    public class Message
    {
        public Guid MessageId { get; set; }

        public String FromName { get; set; }

        public String FromAddress { get; set; }

        public String Subject { get; set; }

        public DateTime DateSent { get; set; }

        public String Body { get; set; }

        public virtual List<Attachment> Attachments { get; set; }

        public Message()
        {
            Attachments = new List<Attachment>();
        }
    }
}
