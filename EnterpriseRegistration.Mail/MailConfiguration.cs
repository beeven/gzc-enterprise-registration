using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EnterpriseRegistration.Mail
{
    public class MailConfiguration: ConfigurationSection
    {
        [ConfigurationProperty("popServer", DefaultValue="172.7.1.2", IsRequired=false)]
        public String PopServer
        {
            get { return (String)this["popServer"]; }
            set {this["popServer"] = value;}
        }

        [ConfigurationProperty("popPort", DefaultValue = 110, IsRequired = false)]
        public int PopPort
        {
            get { return (int)this["popPort"]; }
            set { this["popPort"] = value; }
        }

        [ConfigurationProperty("smtpServer", DefaultValue = "172.7.1.2", IsRequired = false)]
        public String SmtpServer
        {
            get { return (String)this["smtpServer"]; }
            set { this["smtpServer"] = value; }
        }

        [ConfigurationProperty("smtpPort", DefaultValue = 25, IsRequired = false)]
        public int SmtpPort
        {
            get { return (int)this["smtpPort"]; }
            set { this["smtpPort"] = value; }
        }

        [ConfigurationProperty("popUsername", DefaultValue = "beeven.ye@customs.gov.cn#mail.customs.gov.cn", IsRequired = false)]
        public String PopUsername { get { return (String)this["popUsername"]; } set { this["popUsername"] = value; } }

        [ConfigurationProperty("popPassword", DefaultValue = "5001nssi!", IsRequired = false)]
        public String PopPassword { get { return (String)this["popPassword"]; } set { this["popPassword"] = value; } }



        [ConfigurationProperty("smtpUsername", DefaultValue = "beeven.ye@customs.gov.cn", IsRequired = false)]
        public String SmtpUsername { get { return (String)this["smtpUsername"]; } set { this["smptUsername"] = value; } }

        [ConfigurationProperty("smtpPassword", DefaultValue = "5001nssi!", IsRequired = false)]
        public String SmtpPassword { get { return (String)this["smtpPassword"]; } set { this["smtpPassword"] = value; } }

    }
}
