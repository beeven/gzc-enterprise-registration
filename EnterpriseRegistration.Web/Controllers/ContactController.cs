using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnterpriseRegistration.Data;
using EnterpriseRegistration.Web.Models;
using EnterpriseRegistration.UserProfile;

namespace EnterpriseRegistration.Web.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/

        ICustomMapService cusMapService = new CustomMapService();
        ICustomsService cusService = new CustomService();

        public ActionResult Index()
        {
            Contact contact = new Contact();
            List<EnterpriseRegistration.Data.Models.Customs> custlist = cusService.GetAll();
            List<EnterpriseRegistration.Data.Models.CustomMap> custMaplist = cusMapService.GetAll();
            contact.customs = new List<WebCustom>();
            foreach (EnterpriseRegistration.Data.Models.Customs cusItem in custlist)
            {
                List<string> strlist = custMaplist.Where(x => x.MapCode == cusItem.CustomCode).Select(x => x.TopCustomCode).ToList();
                WebCustom webcus = new WebCustom();
                webcus.cust = cusItem;
                webcus.customCodes = string.Join("/", strlist.ToArray());
                contact.customs.Add(webcus);
            }
            contact.codes = new List<string>();
            if (UserProfileContext.CurrentUser.Roles.Select(x=>x.RoleCode).Contains("ADMINISTRATION"))
            {
                contact.codes.AddRange(contact.customs.Select(x => x.cust.CustomCode).ToList());
            }
            else
            {
                string mapCode = cusMapService.GetMapCodeByCustomCode(UserProfileContext.CurrentUser.CustomCode);
                contact.codes.Add(mapCode);
            }
            return View(contact);
        }

        public string Update(string contact, string tel, string id)
        {
            try
            {
                cusService.Update(id, contact, tel);
                return "修改成功！";
            }
            catch (Exception ex)
            {
                return "修改失败！";
            }
        }

    }
}
