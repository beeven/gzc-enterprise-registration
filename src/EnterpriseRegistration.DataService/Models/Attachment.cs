using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.DataService.Models
{
    public class Attachment
    {
        public Guid AttachmentId { get; set; }

        /// <summary>
        /// Name of attachment
        /// </summary>
        public String OriginalName { get; set; }

        public String MIMEType { get; set; }

        public long Size { get; set; }

        public String HashName { get; set; }


        /// <summary>
        /// Reference to AttachmentFile
        /// </summary>
        public Guid stream_id { get; set; }

        public AttachmentFile File { get; set; }


        /// <summary>
        /// Reference to Message
        /// </summary>
        public Guid MessageId { get; set; }

        public Message Message { get; set; }

    }
}
