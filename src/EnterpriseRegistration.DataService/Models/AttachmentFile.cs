using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.DataService.Models
{
    public class AttachmentFile
    {
        public Guid stream_id { get; set; }
        public byte[] file_stream { get; set; }
      
        public String name { get; set; }

    }
}
