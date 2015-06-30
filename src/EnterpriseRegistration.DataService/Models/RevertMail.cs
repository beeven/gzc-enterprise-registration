using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseRegistration.DataService.Models
{
    public class RevertMail
    {

        public String FileName { get; set; }

        public int  RightNum { get; set; }

        public int ErrorNum { get; set; }

        public DateTime InputDate { get; set; }

        public SendStatus? SendFlag { get; set; }

    }

    public enum SendStatus
    {
        Pending = 0,
        Sent = 1,
        Error = 2
    }
}
