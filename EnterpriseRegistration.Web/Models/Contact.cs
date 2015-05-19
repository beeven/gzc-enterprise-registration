using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EnterpriseRegistration.Data.Models;

namespace EnterpriseRegistration.Web.Models
{
    public class Contact
    {
        public List<WebCustom> customs { get; set; }
        public List<String> codes { get; set; }
    }

    public class WebCustom
    {
        public Customs cust { get; set; }
        public string customCodes { get; set; }
    }
}