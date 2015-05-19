using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EnterpriseRegistration.UserProfile.Models;
using gz.Common.OGUPermission;
using EnterpriseRegistration.UserProfile.WebReference;
using EnterpriseRegistration.UserProfile.WebAccreditReference;
using System.Xml;

namespace EnterpriseRegistration.UserProfile
{
    public class UserProfileContext
    {


        public User _currentUser;
        public Models.RoleCollection _role;

        public UserProfileContext()
        {
            InitUser();
            InitRoles();
        }

        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current != null)
                {

                    if (HttpContext.Current.Items["UserProfileContext"] == null)
                    {
                        HttpContext.Current.Items["UserProfileContext"] = new UserProfileContext();
                    }
                    UserProfileContext context = HttpContext.Current.Items["UserProfileContext"] as UserProfileContext;

                    return context._currentUser;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public static Models.RoleCollection Roles
        {
             get
            {
                if (HttpContext.Current != null)
                {

                    if (HttpContext.Current.Items["UserProfileContext"] == null)
                    {
                        HttpContext.Current.Items["UserProfileContext"] = new UserProfileContext();
                    }
                    UserProfileContext context = HttpContext.Current.Items["UserProfileContext"] as UserProfileContext;

                    return context._role;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private void InitUser()
        {

            IUser user = OguAdminMechanismFactory.GetMechanism().GetCurrentUser();
            var config = System.Configuration.ConfigurationManager.GetSection("appConfiguration") as gz.Common.Core.UserConfig.AppConfiguration;
            if (user != null)
            {
                OguReaderService ws = new OguReaderService();
                var roles=user.Roles[config.AppCode];
                _currentUser = new User()
                {
                    Id = user.ID,
                    DisplayName = user.DisplayName,
                    PersonId = user.PersonID,
                    deptName=user.Parent.DisplayName,
                    Roles = roles.Select(x => 
                    {
                        return new Role()
                        {
                            RoleName=x.Name,
                            RoleCode=x.CodeName,
                            RoleId=x.ID
                        };
                    }).ToList()
                };
                user.FullPath = user.FullPath.Replace("海关总署", "中国海关");
                user.FullPath = user.FullPath.Replace("\\" + user.DisplayName, "");
                _currentUser.CustomCode = ws.GetCustomsCodeByUserPath(user.FullPath);     
               
            }
            else
            {
                _currentUser = new User()
                {
                    Id = null,
                    DisplayName = "No login",
                    CustomCode=""
                };
            }
        }

        private void InitRoles()
        {
            AccreditReaderService ws = new AccreditReaderService();
            _role = new Models.RoleCollection();
            _role.Roles = new List<Role>();
            string ret = ws.GetRoles("", WebAccreditReference.AccreditCategory.Code, "ENT_REG", WebAccreditReference.AccreditCategory.Code, "BASE_VIEW", WebAccreditReference.AccreditCategory.Code, WebAccreditReference.RoleCategories.All, "*");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ret);
            XmlNode node = doc.ChildNodes[0];
            foreach (XmlNode child in node)
            {
                if (child.ChildNodes[3].InnerText != "APP_ADMIN" && child.ChildNodes[3].InnerText != "SYSTEM_ADMIN" && child.ChildNodes[3].InnerText != "ADMINISTRATION")
                {
                    Role rolechild = new Role()
                    {
                        RoleId = child.ChildNodes[0].InnerText,
                        RoleCode = child.ChildNodes[3].InnerText,
                        RoleName = child.ChildNodes[2].InnerText
                    };
                    _role.Roles.Add(rolechild);
                }
            }
        }

    }
}
