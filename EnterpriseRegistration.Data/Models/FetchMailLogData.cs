using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data.Models
{
    public class FetchMailLogData
    {
        public int Count{get;set;}
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
        public List<SystemLog> logs { get; set; }
    }
}
