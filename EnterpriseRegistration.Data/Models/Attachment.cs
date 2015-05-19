using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;


namespace EnterpriseRegistration.Data.Models
{
    public class Attachment
    {
        [Key]
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String MIME { get; set; }
        public long Size { get; set; }
        
    }
}
