using EnterpriseRegistration.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EnterpriseRegistration.Web.Models
{
    public class DataStatisticQuery
    {
        public int ListCount { get; set; }
        public int TotalPage { get; set; }
        public DataTable dtItem { get; set; }
    }
}