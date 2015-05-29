using EnterpriseRegistration.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Web.Models
{
    public class TableViewModel
    {
        public IEnumerable<Message> Messages { get; set; }

        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }

        public int CurrentId { get; set; }
    }
}
