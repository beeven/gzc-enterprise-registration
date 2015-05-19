using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseRegistration.Mail.Models;

namespace EnterpriseRegistration.Mail
{
    public interface IMailService
    {
        List<Models.Mail> FetchMails();

        string  Reply(string content, string subject, string to);

        string ExcelReply(string strExcelReply, string subject, string to);
    }
}
