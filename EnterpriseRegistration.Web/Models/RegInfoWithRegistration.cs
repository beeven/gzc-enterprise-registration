using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EnterpriseRegistration.Data.Models;

namespace EnterpriseRegistration.Web.Models
{
    public class RegInfoWithRegistration
    {
        public RegInfo regInfo { get; set; }
        public Registration registration { get; set; }
        public Customs custom { get; set; }
    }
}