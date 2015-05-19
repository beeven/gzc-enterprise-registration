using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Data = EnterpriseRegistration.Data;
using Mail = EnterpriseRegistration.Mail;
using System.IO;
using System.Data;
using System.Configuration;

namespace EnterpriseRegistration.MailService.Console
{
    class Program
    {
        static IUnityContainer uContainer = new UnityContainer();
        static Data.IFileStorageService fileStorageService;
        static Data.IRegInfoService regInfoService;
        static Mail.IMailService mailService;
        static Data.IRegistrationService registrationService;
        static Data.ISystemLogService systemLogService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program");
        static bool ReplyFlag = bool.Parse(ConfigurationManager.AppSettings["ReplyFlag"]);
        static void Initialize()
        {
            uContainer.RegisterType<EnterpriseRegistration.Data.IFileStorageService, EnterpriseRegistration.Data.LocalFileStorageService>();
            uContainer.RegisterType<EnterpriseRegistration.Data.IRegInfoService, EnterpriseRegistration.Data.MSSQLRegInfoService>();
            uContainer.RegisterType<EnterpriseRegistration.Mail.IMailService, EnterpriseRegistration.Mail.MailService>();
            uContainer.RegisterType<EnterpriseRegistration.Data.IRegistrationService, EnterpriseRegistration.Data.RegistrationService>();
            uContainer.RegisterType<EnterpriseRegistration.Data.ISystemLogService, EnterpriseRegistration.Data.SystemLogService>();
            fileStorageService = uContainer.Resolve<Data.IFileStorageService>();
            regInfoService = uContainer.Resolve<Data.IRegInfoService>();
            mailService = uContainer.Resolve<Mail.MailService>();
            registrationService = uContainer.Resolve<Data.IRegistrationService>();
            systemLogService = uContainer.Resolve<Data.ISystemLogService>();
        }

        static Data.Models.RegInfo Cast(Mail.Models.Mail mail)
        {
            Data.Models.RegInfo regInfo = new Data.Models.RegInfo()
            {
                From = mail.From,
                Body = mail.Body,
                Subject = mail.Subject,
                IsReplied = false,
                ReceivedDate = DateTime.Now,
                Attachments = new List<Data.Models.Attachment>()
            };
            if (mail.Attachments != null)
            {
                foreach (var att in mail.Attachments)
                {
                    var id = fileStorageService.Upload(new MemoryStream(att.Content));
                    regInfo.Attachments.Add(new Data.Models.Attachment()
                    {
                        Id = id,
                        Name = att.Name,
                        Size = att.Content.LongLength,
                        MIME = att.MIME
                    });
                }
            }
            return regInfo;
        }

        static void AddLog(string context, string mailUser, int type, int? recordId, string Operator)
        {
            Data.Models.SystemLog log = new Data.Models.SystemLog();
            log.Id=Guid.NewGuid();
            log.LogContext=context;
            log.CreateTime=DateTime.Now;
            log.MailUser=mailUser;
            log.type = type;
            log.RecordID = recordId;
            log.Operator = Operator;
            systemLogService.Save(log);
        }

        /// <summary>
        /// 解析Excel存入到数据库
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="AttchID"></param>
        static void AddRegistration(Mail.Models.Mail mail)
        {
            string strExcelReply = "";
            //判断是否有附件，没有则写入日志
            if (mail.Attachments.Count > 0)
            {
                int excelFlag = 0;
                //循环所有附件
                foreach (var item in mail.Attachments)
                {
                    //判断分析pdf
                    if (item.Name.ToLower().EndsWith(".pdf"))
                    {
                        //有备案申请文件改为1
                        excelFlag = 1;
                        Dictionary<string, object> dict=null;
                        try
                        {
                            //获取解析的pdf结果
                            dict = registrationService.AnalysisPDF(item.Content);
                        }
                        catch (Exception ex)
                        {
                            if (ReplyFlag)
                            {
                                strExcelReply = MailExcelReply("\\邮件文言\\分析pdf出错.txt");
                                //分析出错，回复邮件
                                mailService.ExcelReply(strExcelReply, mail.Subject + "--回执", mail.From);
                            }
                            string error = "PDF分析不通过。AnalysisPDF方法出现异常:" + ex.Message + "\r\n堆栈信息：" + ex.StackTrace + "\r\n";
                            log.Error(error);
                            AddLog(error, mail.From, 2,null,"计划任务");
                            break;
                        }
                        var reginfo = Cast(mail);
                        try
                        {
                            if (dict.Count > 0)
                            {
                                regInfoService.Save(reginfo);
                                //存入数据库
                                registrationService.InsertPDF(dict, reginfo.Id);
                                //正常日志
                                string strPass = "PDF信息已经正常导入到数据库" + "\r\n";
                                log.Info(strPass);
                                AddLog(strPass, mail.From, 0, reginfo.Id, "计划任务");
                                break;
                            }
                            else
                            {
                                string strPass ="申请表PDF不符合规范，发件人:"+mail.From + "\r\n";
                                log.Info(strPass);
                                AddLog(strPass, mail.From, 1, null, "计划任务");
                            }
                        }
                        catch (Exception ex)
                        {
                            //删除记录
                            regInfoService.Delete(reginfo.Id);
                            if (ReplyFlag)
                            {
                                //发送邮件
                                strExcelReply = MailExcelReply("\\邮件文言\\下载最新备案申请表.txt");
                                mailService.ExcelReply(strExcelReply, mail.Subject + "--回执", mail.From);
                            }
                            //插入数据出错，写入日志
                            string error = "PDF信息未正常入库:" + ex.Message + "\r\n堆栈信息：" + ex.StackTrace + "\r\n";
                            log.Error(error);
                            AddLog(error, mail.From, 2, null, "计划任务");
                            break;
                        }

                    }
                    ////判断分析excel
                    //if (item.Name.ToLower().EndsWith(".xls") || item.Name.ToLower().EndsWith(".xlsx"))
                    //{
                    //    //有备案申请文件改为1
                    //    excelFlag = 1;
                    //    DataTable dt = null;
                    //    try
                    //    {
                    //        //将附件转换成流的形式
                    //        Stream sm = BytesToStream(item.Content);
                    //        //分析Excel
                    //        dt = registrationService.AnalysisExcel(sm, item.Name);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        if (ReplyFlag)
                    //        {
                    //            strExcelReply = MailExcelReply("\\邮件文言\\分析Excel出错.txt");
                    //            //分析出错，回复邮件
                    //            mailService.ExcelReply(strExcelReply, mail.Subject + "--回执", mail.From);
                    //        }
                    //        string error = "Excel分析不通过。AnalysisExcel方法出现异常:" + ex.Message + "\r\n堆栈信息：" + ex.StackTrace + "\r\n";
                    //        log.Error(error);
                    //        AddLog(error, mail.From, 2);
                    //        break;
                    //    }
                    //    var reginfo = Cast(mail);
                    //    try
                    //    {
                    //        if (dt != null)
                    //        {
                    //            regInfoService.Save(reginfo);
                    //            //存入数据库
                    //            registrationService.InsertExcel(dt, reginfo.Id);
                    //            //正常日志
                    //            string strPass = "Excel信息已经正常导入到数据库" + "\r\n";
                    //            log.Info(strPass);
                    //            AddLog(strPass, mail.From, 0);
                    //            break;
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        //删除记录
                    //        regInfoService.Delete(reginfo.Id);
                    //        if (ReplyFlag)
                    //        {
                    //            //发送邮件
                    //            strExcelReply = MailExcelReply("\\邮件文言\\下载最新备案申请表.txt");
                    //            mailService.ExcelReply(strExcelReply, mail.Subject + "--回执", mail.From);
                    //        }
                    //        //插入数据出错，写入日志
                    //        string error = "Excel信息未正常入库:" + ex.Message + "\r\n堆栈信息：" + ex.StackTrace + "\r\n";
                    //        log.Error(error);
                    //        AddLog(error, mail.From, 2);
                    //        break;
                    //    }
                    //}
                }
                
                if (excelFlag == 0)
                {
                    if (ReplyFlag)
                    {
                        strExcelReply = MailExcelReply("\\邮件文言\\没有备案申请附件.txt");
                        //循环所有附件未找到备案申请表
                        mailService.ExcelReply(strExcelReply, mail.Subject + "--回执", mail.From);
                    }
                    //没有备案申请附件，写入日志
                    string error = "没有备案申请附件！" + "\r\n";
                    log.Error(error);
                    AddLog(error, mail.From, 1, null, "计划任务");
                }
            }
            else
            {
                //没有任何附件写入日志
                if (ReplyFlag)
                {
                    strExcelReply = MailExcelReply("\\邮件文言\\没有附件.txt");
                    mailService.ExcelReply(strExcelReply, mail.Subject + "--回执", mail.From);
                }
                string error = "没有任何附件！" + "\r\n";
                log.Error(error);
                AddLog(error, mail.From, 1, null, "计划任务");
            }
        }



        static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        static string MailExcelReply(string fileName)
        {
            string appPath = Directory.GetCurrentDirectory();
            string path = appPath.Substring(0, appPath.IndexOf("\\bin"));
            string strPath = path + fileName;
            FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string strExcelReply = sr.ReadToEnd();
            return strExcelReply;
        }

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Initialize();
            ConfigurationManager.RefreshSection("mailSetting");//新加刷新获取最新配置邮件地址
            foreach (var mail in mailService.FetchMails())
            {
                if (ReplyFlag)
                {
                    string strExcelReply = MailExcelReply("\\邮件文言\\审核通知.txt");
                    mailService.ExcelReply(strExcelReply, mail.Subject + "--回执", mail.From);
                }
                //解析备案申请表存入到数据库
                AddRegistration(mail);
            }
        }
    }
}
