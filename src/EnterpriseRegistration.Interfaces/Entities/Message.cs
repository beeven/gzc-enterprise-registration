using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Interfaces.Entities
{
    public class Message
    {
        public Guid Id { get; set; }

        public String From { get; set; }

        public String Subject { get; set; }

        public String Body { get; set; }

        public virtual IEnumerable<Attachment> Attachments { get; set; }
    }
}
