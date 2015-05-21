using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseRegistration.Interfaces.Entities
{
    public class Message
    {
        [ScaffoldColumn(false)]
        public Guid MessageId { get; set; }


        public String From { get; set; }

        public String Subject { get; set; }

        public DateTime DateReceived { get; set; }

        public String Body { get; set; }

        public virtual List<Attachment> Attachments { get; set; }

    }
}
