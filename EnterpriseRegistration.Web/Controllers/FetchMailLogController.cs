using EnterpriseRegistration.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnterpriseRegistration.Data.Models;

namespace EnterpriseRegistration.Web.Controllers
{
    public class FetchMailLogController : Controller
    {
        //
        // GET: /FetchMailLog/
        [RoleAttribute]
        public ActionResult Index()
        {
            return View();
            //ISystemLogService syslogservice = new SystemLogService();
            //List<SystemLog> logs = syslogservice.GetAll().ToList();
            //FetchMailLogData result = new FetchMailLogData();
            //int pagesize = 20;
            //result.PageIndex = pageIndex;
            //result.Count = logs.Count();
            //result.TotalPage = result.Count / pagesize+result.Count%pagesize==0?0:1;
            //result.logs = new List<SystemLog>();
            //for (int i = pagesize * pageIndex; i < pagesize * (pageIndex + 1); i++)
            //{
            //    if (i >= logs.Count())
            //    {
            //        break;
            //    }
            //    result.logs.Add(logs.ElementAt(i));
            //}
            //return View("Index", result);
        }

        public ActionResult Loglist(int pagesize, int currentpage,string type)
        {
            ISystemLogService syslogservice = new SystemLogService();
            List<SystemLog> logs = new List<SystemLog>();
            if(type=="all")
                logs=syslogservice.GetAll();
            else
                logs = syslogservice.GetByTypes(type);
            FetchMailLogData result = new FetchMailLogData();
            result.PageIndex = currentpage;
            result.Count = logs.Count();
            result.TotalPage = result.Count / pagesize + (result.Count % pagesize == 0 ? 0 : 1);
            result.logs = new List<SystemLog>();
            for (int i = pagesize * currentpage; i < pagesize * (currentpage + 1); i++)
            {
                if (i >= logs.Count())
                {
                    break;
                }
                result.logs.Add(logs.ElementAt(i));
            }
            return View("Loglist", result);

        }

    }
}
