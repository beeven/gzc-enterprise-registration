using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnterpriseRegistration.Data;
using EnterpriseRegistration.Data.Models;
using EnterpriseRegistration.UserProfile;
using EnterpriseRegistration.UserProfile.Models;
namespace EnterpriseRegistration.Web.Controllers
{
    public class SetController :Controller
    {
        //
        // GET: /Set/
        [RoleAttribute]
        public ActionResult Index()
        {
            if (!UserProfileContext.CurrentUser.Roles.Select(x => x.RoleCode).Contains("ADMINISTRATION"))
                return RedirectToAction("Index", "RegInfoes");

            RoleCollection result = UserProfileContext.Roles;
            return View(result);
        }

        public string Query(string roleId)
        {
            string resultHtml = "";
            ICustomsService cService = new CustomService();
            ICustomRightsService cRightService = new CustomRightService();
            string customCode = UserProfileContext.CurrentUser.CustomCode;
            List<Customs> c = cService.GetAll();
            List<CustomRights> cRight = cRightService.GetByCustomCode(roleId);
            c.Remove(c.Where(x => x.CustomCode == customCode).FirstOrDefault());
            foreach (Customs cModel in c)
            {
                if(cRight.Select(x=>x.AvailableCustomCode).Contains(cModel.CustomCode))
                    resultHtml += "<input checked='checked' type='checkbox' value='" + cModel.CustomName + "' code='" + cModel.CustomCode + "' />"+cModel.CustomName+"<br/>";
                else
                    resultHtml += "<input type='checkbox' value='" + cModel.CustomName + "' code='" + cModel.CustomCode + "' />" + cModel.CustomName + "<br/>";
            }
            return resultHtml;
        }

        public string Update(string customCode, string cRights)
        {
            try
            {
                ICustomRightsService cRightService = new CustomRightService();
                List<string> newRights = new List<string>(cRights.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                cRightService.UpdateCustomRights(customCode, newRights);
                return "修改成功！";
            }
            catch (Exception ex)
            {
                return "修改不成功，请稍后再试！";
            }
        }

    }
}
