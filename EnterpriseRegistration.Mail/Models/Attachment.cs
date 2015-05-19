using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Mail.Models
{
    public class Attachment
    {
        public String Name { get; set; }
        public byte[] Content { get; set; }
        public String MIME { get; set; }
    }
}
