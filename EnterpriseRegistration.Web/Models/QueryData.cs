using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Web.Models
{
   public class QueryData
    {
        public int ListCount { get; set; }
        public List<RegInfoWithRegistration> ListItem { get; set; }
        public string rolelist { get; set; }
    }
}
