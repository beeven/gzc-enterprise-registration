using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPop.Mime;
using OpenPop.Common;
using OpenPop.Pop3;
using System.Configuration;
using System.Net.Mail;
using System.IO;
using Microsoft.Practices.Unity;

namespace EnterpriseRegistration.Mail
{
    public class MailService : IMailService
    {
        MailConfiguration settings = ConfigurationManager.GetSection("mailSetting") as MailConfiguration;
        bool delMail = bool.Parse(ConfigurationManager.AppSettings["deleteMail"]);
        static Data.ISystemLogService systemLogService;
        static IUnityContainer uContainer = new UnityContainer();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("MailService");
        public List<Models.Mail> FetchMails()
        {
            uContainer.RegisterType<EnterpriseRegistration.Data.ISystemLogService, EnterpriseRegistration.Data.SystemLogService>();
            systemLogService = uContainer.Resolve<Data.ISystemLogService>();
            using (Pop3Client client = new Pop3Client())
            {
                try
                {
                    client.Connect(settings.PopServer, settings.PopPort, false);
                    client.Authenticate(settings.PopUsername, settings.PopPassword);
                }
                catch (Exception ex)
                {
                    string error = "连接邮件服务器出现异常。异常信息:" + ex.Message + "\r\n堆栈信息：" + ex.StackTrace + "\r\n";
                    log.Error(error);
                    AddLog(error, null, 2, null, "计划任务");
                    return null;
                }
                var infos = client.GetMessageInfos();
                int count = client.GetMessageCount();
                List<Models.Mail> ret = new List<Models.Mail>(count);
                for (int i = 0; i < count; i++)
                {
                    //byte[] text = client.GetMessageAsBytes(i + 1);
                    //FileStream st = new FileStream("D:\\error.html",FileMode.Create,FileAccess.Write);

                    //st.Write(text,0,text.Length);
                    //st.Close();
                    try
                    {
                        Message msg = client.GetMessage(i + 1);
                        ret.Add((Models.Mail)msg);
                    }
                    catch (FormatException ex)
                    {
                        string error = "收邮件时,FormatException异常。异常信息:" + ex.Message + "\r\n堆栈信息：" + ex.StackTrace + "\r\n";
                        log.Error(error);
                        AddLog(error, null, 2, null, "计划任务");
                        continue;
                    }
                    catch (InvalidDataException ex)
                    {
                        string error = "收邮件时,InvalidDataException异常。异常信息:" + ex.Message + "\r\n堆栈信息：" + ex.StackTrace + "\r\n";
                        log.Error(error);
                        AddLog(error, null, 2, null, "计划任务");
                        continue;
                    }
                    catch (Exception ex)
                    {
                        string error = "收邮件时,Exception异常。异常信息:" + ex.Message + "\r\n堆栈信息：" + ex.StackTrace + "\r\n";
                        log.Error(error);
                        AddLog(error, null, 2, null, "计划任务");
                        continue;
                    }
                }
                if (delMail)
                {
                    client.DeleteAllMessages();
                }
                return ret;


            }
        }

        static void AddLog(string context, string mailUser, int type,int? recordId,string Operator)
        {
            Data.Models.SystemLog log = new Data.Models.SystemLog();
            log.Id = Guid.NewGuid();
            log.LogContext = context;
            log.CreateTime = DateTime.Now;
            log.MailUser = mailUser;
            log.type = type;
            log.RecordID = recordId;
            log.Operator = Operator;
            systemLogService.Save(log);
        }

        public string Reply(string content, string subject, string to)
        {
            try
            {
                SmtpClient client = new SmtpClient(settings.SmtpServer);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(settings.SmtpUsername, settings.SmtpPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailAddress addressFrom = new MailAddress(settings.SmtpUsername);
                MailAddress addressTo = new MailAddress(to);
                MailMessage message = new MailMessage(addressFrom, addressTo);
                message.Sender = addressFrom;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Body = content;
                message.Subject = subject;
                client.Send(message);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ExcelReply(string content, string subject, string to)
        {
            try
            {
                SmtpClient client = new SmtpClient(settings.SmtpServer);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(settings.SmtpUsername, settings.SmtpPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailAddress addressFrom = new MailAddress(settings.SmtpUsername);
                MailAddress addressTo = new MailAddress(to);
                MailMessage message = new MailMessage(addressFrom, addressTo);
                message.Sender = addressFrom;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = false;
                message.Body = content;
                message.Subject = subject;
                client.Send(message);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
