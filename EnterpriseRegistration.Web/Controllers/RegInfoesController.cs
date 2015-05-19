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
    public class RegInfoesController : Controller
    {
        User currentUser = UserProfileContext.CurrentUser;

        static IRegInfoService regInfoService = new MSSQLRegInfoService();
        static ISystemLogService sysService = new SystemLogService();
        static IRegistrationService registrationService = new RegistrationService();
        static ICustomsService cusService = new CustomService();
        IMailService mailservice = new MailService();
        //
        // GET: /RegInfoes/
        [RoleAttribute]
        public ActionResult Index()
        {
            return View(currentUser);
        }

        public ActionResult User()
        {
            User user = UserProfileContext.CurrentUser;
            return PartialView("PartialUser", user);
        }

        //public void Edit()
        //{
        //    Response.Redirect("http://10.99.101.200/PassPort/LogOff.aspx");
        //}

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult Query(int pagesize, int currentpage, string time, string filter)
        {

            List<RegInfo> item = new List<RegInfo>();
            if (!currentUser.Roles.Select(x => x.RoleCode).Contains("ADMINISTRATION"))
            {
                item = regInfoService.GetAllByUserCustomCode(currentUser.Roles.Select(x => x.RoleId).ToList(), currentUser.CustomCode); //regService.GetAll().ToList();
            }
            else
            {
                item = regInfoService.GetAll();
            }
            List<RegInfoWithRegistration> newItems = new List<RegInfoWithRegistration>();
            List<RegInfoWithRegistration> resultItems = new List<RegInfoWithRegistration>();
            List<Customs> allCustoms=cusService.GetAll();
            foreach (RegInfo regItem in item)
            {
                RegInfoWithRegistration regwithregistration = new RegInfoWithRegistration()
                {
                    regInfo = regItem,
                    registration = registrationService.GetByAttchID(regItem.Id)

                };
                if (regwithregistration.registration != null)
                {
                    regwithregistration.custom = allCustoms.Where(x => x.CustomCode == regwithregistration.registration.CustomsName).FirstOrDefault();
                    if (regwithregistration.custom != null)
                    {
                        if (filter == "")
                        {
                            newItems.Add(regwithregistration);
                        }
                        else
                        {
                            if (regwithregistration.registration.EnterPriseName.Contains(filter))
                            {
                                newItems.Add(regwithregistration);
                            }
                            else if (regItem.RecordNum != null)
                            {
                                if (regItem.RecordNum.Contains(filter))
                                {
                                    newItems.Add(regwithregistration);
                                }
                            }

                        }
                       
                    }
                }
            }
            
            for (int i = pagesize * currentpage; i < pagesize * (currentpage + 1); i++)
            {
                if (i >= newItems.Count())
                {
                    break;
                }
                resultItems.Add(newItems[i]);
              
                
            }
            QueryData data = new QueryData();
            data.ListCount = newItems.Count;
            data.ListItem = resultItems;
            return PartialView("MailList", data);
        }



        public string GetEntAccess(string replyId,string to)
        {
            try
            {
                RegInfo regInfo = new RegInfo();
                regInfo = regInfoService.Get(int.Parse(replyId));
                object[] obj = new object[2];
                Registration regs = new Registration();
                regs = registrationService.GetByAttchID(int.Parse(replyId));

                string content = "您的跨境企业备案申请已受理，请耐心等待。";
                string result = mailservice.Reply(content, regInfo.Subject+"--受理回复", to);

                List<Registration> registrationsByName = registrationService.GetByEnterPriseName(regs.EnterPriseName);
                bool isRecordPassed = false;
                string recordPassedCustomName = "";
                string record = "";
                foreach (Registration registrationModel in registrationsByName)
                {
                    RegInfo regmodel = regInfoService.Get(registrationModel.AttchID);
                    if (regmodel.isGetEntAccess == 1 && regmodel.isRecordPass == 1)
                    {
                        isRecordPassed = true;
                        recordPassedCustomName =cusService.GetAll().Where(x=>x.CustomCode==registrationModel.CustomsName).Select(x=>x.CustomName).ElementAtOrDefault<string>(0);
                        record = regmodel.RecordNum;
                        break;
                    }
                }
                if (isRecordPassed)
                {
                    obj[0] = "0";
                    obj[1] = "该企业已在" + recordPassedCustomName + "办理企业备案手续。备案号为："+record;
                }
                else
                {
                    WebEntAccess ws = new WebEntAccess();
                    EntAccessDateClass _EntAccessDataClass = DataToWebModel(regs);
                    obj = ws.GetEntAccessData(_EntAccessDataClass);
                }
                regInfo.isGetEntAccess = 1;
                regInfo.isRecordPass = int.Parse(obj[0].ToString());
                regInfo.RecordNum = obj[1].ToString();
                regInfo.GetEntAccessDate = DateTime.Now;
                regInfoService.Update(regInfo);
                string strReplay = "";
                if (isRecordPassed)
                {
                    strReplay= "该企业已在" + recordPassedCustomName + "办理企业备案手续";
                }
                else
                {
                    if (regInfo.isRecordPass == 1)
                        strReplay= "备案通过";
                    else
                        strReplay= "备案不通过";
                }
                //添加日志
                SystemLog log = new SystemLog()
                {
                    Id = Guid.NewGuid(),
                    LogContext ="申请备案\""+regInfo.Subject+"\",备案结果为："+ strReplay,
                    CreateTime = DateTime.Now,
                    MailUser = currentUser.DisplayName + "(" + currentUser.deptName + ")",
                    type = 4,
                    RecordID = int.Parse(replyId),
                    Operator = currentUser.DisplayName
                };
                sysService.Save(log);
                return strReplay;
            }
            catch (Exception ex)
            {
                return "系统操作失败，请稍后再试!";
            }
        }

        public string Pass(string replyId, string content, string to, string subject)
        {
            int id = int.Parse(replyId);
            subject = HttpUtility.UrlDecode(subject);
            string subject1 = subject + "--回复";
            to = HttpUtility.UrlDecode(to);
            //content = "您申请的备案已通过审核<br/>" + HttpUtility.UrlDecode(content) + "<br/>请与海关相关技术人员联系获取相关技术资料，并开展数据对接联调。<br/>技术人员联系电话：13316083120 ，020-84104658 ，邮箱：2972918402@qq.com ，联系人：林小姐<br/>";
            content = HttpUtility.UrlDecode(content) + "请与海关相关技术人员联系获取相关技术资料，并开展数据对接联调。<br/>技术人员联系电话：13316083120 ，020-84104658 ，邮箱：2972918402@qq.com ，联系人：林小姐<br/>";
            content=content.Replace("\n", "<br/>");
            string customName = registrationService.GetByAttchID(int.Parse(replyId)).CustomsName;
            //List<Customs> cus = cusService.GetAll();
            //foreach (Customs item in cus)
            //{
            //    if (item.CustomName == customName)
            //    {
            //        content += "联络员：" + item.Contact + " ； 联系电话 ：" + item.Tel;
            //        break;
            //    }
            //}
            string result = mailservice.Reply(content, subject1, to);
            if (result == "")
            {
               
                regInfoService.SetReplied(id, content);
                SystemLog log = new SystemLog()
                {
                    Id = Guid.NewGuid(),
                    LogContext = currentUser.DisplayName + "通过了申请邮件\"" + subject + "\"",
                    CreateTime = DateTime.Now,
                    MailUser = currentUser.DisplayName+"("+currentUser.deptName+")",
                    type = 3,
                    RecordID=id,
                    Operator = currentUser.DisplayName
                };
                sysService.Save(log);
                return "回复成功！";
            }
            else
            {
                return result;
            }
        }

        public string Reback(string replyId, string content, string to, string subject)
        {
            IMailService mailservice = new MailService();
            int id = int.Parse(replyId);
            subject = HttpUtility.UrlDecode(subject);
            string subject1 = subject + "--回复";
            to = HttpUtility.UrlDecode(to);
            //content = "您申请的备案未通过审核；<br/>原因为：" + HttpUtility.UrlDecode(content) + "<br/>";
            content =  HttpUtility.UrlDecode(content) + "<br/>";
            content = content.Replace("\n", "<br/>");

            try
            {
                RegInfo info = regInfoService.Get(id);
                info.isRecordPass = 0;
                info.RecordNum = content;
                info.IsReplied = true;
                info.RepliedBody = content;
                info.RepliedDate = DateTime.Now;
                regInfoService.Update(info);
                //regInfoService.SetReplied(id, content);
                SystemLog log = new SystemLog()
                {
                    Id = Guid.NewGuid(),
                    LogContext = currentUser.DisplayName + "退回了申请邮件\"" + subject + "\"",
                    CreateTime = DateTime.Now,
                    MailUser = currentUser.DisplayName + "(" + currentUser.deptName + ")",
                    type = 3,
                    RecordID=id,
                    Operator = currentUser.DisplayName
                };
                sysService.Save(log);
                string result = mailservice.Reply(content, subject1, to);
                return "回复成功！";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
           
        }
        public static EntAccessDateClass DataToWebModel(Data.Models.Registration source)
        {
            string _ENT_TYPE = "";
            if (source.ManagementFlag ==0)
                _ENT_TYPE += "01,";
            if (source.TransactionFlag == 0)
                _ENT_TYPE += "02,";
            if (source.LogisticsFlag ==0)
                _ENT_TYPE += "03,";
            if (source.PayFlag == 0)
                _ENT_TYPE += "04,";

            string _BUSINESS_TYPE = "";
            if (source.OnlineFlag ==0)
                _BUSINESS_TYPE += "B2B2C,";
            if (source.DirectFlag == 0)
                _BUSINESS_TYPE += "B2C,";
            EntAccessDateClass result = new EntAccessDateClass()
            {
                BUSINESS_LICENSENO = source.LicenseID,
                ENT_ABBREVIATION = source.EnterPriseAbbreviation,
                BUSINESS_TYPE = _BUSINESS_TYPE,
                ENT_ADDR = source.EnterPriseAddress,
                ENT_PERSON = source.EnterPriseContact,
                ENT_PHONE = source.EnterPrisePhone,
                ENT_TYPE = _ENT_TYPE,
                IS_ENTABROAD = source.Abroad.ToString(),
                LEGAL_IDCARD = source.LegalID,
                LEGAL_PERSON = source.LegalName,
                LOG_TRACK_WEBSITE = source.LogisticsWebsite,
                MASTER_CUSTOMS = source.CustomsName,
                NETWORK_NAME = source.TransactionName,
                NETWORK_WEBSITE = source.TransactionWebsite,
                NOTE = source.Remark,
                ORG_CODE = source.OrgCode,
                TAX_REGISTRATION_CODE = source.TaxCode,
                TRADE_CODE = source.CustomsID,
                TRADE_NAME = source.EnterPriseName //registrtion_rjktest2
            };
            return result;

        }
    }
}
