using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Interfaces.Entities
{
    public class Attachment
    {
        public Guid AttachmentId { get; set; }

        public String FileName { get; set; }

        public String MIMEType { get; set; }

        public byte[] Content { get; set; }

        public Guid MessageId { get; set; }
        public Message Message { get; set; }
    }
}
