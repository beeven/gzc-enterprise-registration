using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Interfaces.Entities
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public String FileName { get; set; }

        public String MIMEType { get; set; }

        public long Size { get; set; }

        public byte[] Content { get; set; }
    }
}
