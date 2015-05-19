using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnterpriseRegistration.Data;
using EnterpriseRegistration.Data.Models;
using EnterpriseRegistration.Mail;
using EnterpriseRegistration.Web.WebEntReference;
using System.Web.Script.Serialization;
using EnterpriseRegistration.UserProfile;
using EnterpriseRegistration.UserProfile.Models;
using EnterpriseRegistration.Web.Models;

namespace EnterpriseRegistration.Web.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        static IRegistrationService registrationService = new RegistrationService();
        static ICustomsService cusService = new CustomService();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Query(int pagesize, int currentpage)
        {
            IRegInfoService regService = new MSSQLRegInfoService();

            List<RegInfo> item = regService.GetAll();
            List<RegInfoWithRegistration> newItems = new List<RegInfoWithRegistration>();
            List<Customs> allCustoms = cusService.GetAll();
            for (int i = pagesize * currentpage; i < pagesize * (currentpage + 1); i++)
            {
                if (i >= item.Count())
                {
                    break;
                }
                RegInfoWithRegistration regwithregistration = new RegInfoWithRegistration()
                {
                    regInfo = item[i],
                    registration = registrationService.GetByAttchID(item[i].Id)
                };
                regwithregistration.custom = allCustoms.Where(x => x.CustomCode == regwithregistration.registration.CustomsName).FirstOrDefault();

                newItems.Add(regwithregistration);
            }
            QueryData data = new QueryData();
            data.ListCount = item.Count;
            data.ListItem = newItems;
            return PartialView("MailList", data);
        }

    }
}
