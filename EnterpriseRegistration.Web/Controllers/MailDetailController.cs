using EnterpriseRegistration.Data;
using EnterpriseRegistration.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EnterpriseRegistration.Web.Controllers
{
    public class MailDetailController : Controller
    {
        //
        // GET: /MailDetail/

        IRegInfoService regService = new MSSQLRegInfoService();
        IRegistrationService regservice = new RegistrationService();
        IFileStorageService fss = new LocalFileStorageService();
        ICustomsService cusservice = new CustomService();
        public ViewResult Index(int id)
        {
            
            RegInfo result = regService.Get(id);
            return View("MailDetail",result);
        }

        public void DownLoad(string id, string mimetype, string displayName)
        { 
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(displayName));
            fss.Download(Response.OutputStream, Guid.Parse(id));

        }

        public string GetApplyTable(string replyId)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            EnterpriseRegistration.Data.Models.Registration regs = new EnterpriseRegistration.Data.Models.Registration();
            regs = regservice.GetByAttchID(int.Parse(replyId));
            List<string> customnames = cusservice.GetAll().Where(x => x.CustomCode == regs.CustomsName).ToList().Select(x=>x.CustomName).ToList();
            if (customnames.Count > 0)
            {
                regs.CustomsName = customnames.ElementAt<string>(0);
            }
            else
            {
                regs.CustomsName = "";
            }
            string result=jss.Serialize(regs);
            return result;
        }

       

    }
}
