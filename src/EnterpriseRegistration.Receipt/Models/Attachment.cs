using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Receipt.Models
{
    public class Attachment
    {
        public Guid AttachmentId { get; set; }

        public String OriginalName { get; set; }

        public String HashName { get; set; }

        public String MIMEType { get; set; }

        public Guid stream_id { get; set; }
        public AttachmentFile File { get; set; }

        public Guid MessageId { get; set; }
        public Message Message { get; set; }
    }
}
