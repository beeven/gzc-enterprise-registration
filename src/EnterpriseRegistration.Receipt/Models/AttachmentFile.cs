using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseRegistration.Receipt.Models
{
    public class AttachmentFile
    {
        [Required]   
        public Guid stream_id { get; set; }

        public byte[] file_stream { get; set; }

        [MaxLength(255)]
        [Required]
        public String name { get; set; }

        
        public DateTime creation_time { get; set; }

        public DateTime last_write_time { get; set; }

        public DateTime? last_access_time { get; set; }

        public bool is_directory { get; set; }

        public bool is_offline { get; set; }

        public bool is_hidden { get; set; }

        public bool is_readonly { get; set; }

        public bool is_archive { get; set;}

        public bool is_system { get; set; }

        public bool is_temporary { get; set; }
    }
}
