using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace EnterpriseRegistration.MailService.Service
{
    partial class FetchMailService : ServiceBase
    {
        private int loopInterval = 60 * 1000;
        static String loop = System.Configuration.ConfigurationManager.AppSettings["loopInterval"];
        static bool ReplyFlag = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["ReplyFlag"]);
        static Data.IFileStorageService fileStorageService;
        static Data.IRegInfoService regInfoService;
        static Mail.IMailService mailService;
        static Data.IRegistrationService registrationService;
        static Data.ISystemLogService systemLogService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program");
        public FetchMailService()
        {
            InitializeComponent();
           
            
        }

        protected override void OnStart(string[] args)
        {
            this.EventLog.WriteEntry("FetMailService starting");
            log4net.Config.XmlConfigurator.Configure();
            fileStorageService = new EnterpriseRegistration.Data.LocalFileStorageService();
            regInfoService = new EnterpriseRegistration.Data.MSSQLRegInfoService();
            mailService = new EnterpriseRegistration.Mail.MailService();
            registrationService = new EnterpriseRegistration.Data.RegistrationService();
            systemLogService = new EnterpriseRegistration.Data.SystemLogService();
            Mail();

            Timer timer1 = new Timer();
            timer1.Elapsed += timer1_Elapsed;
            timer1.Interval = int.Parse(loop)*loopInterval;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.EventLog.WriteEntry("timer1 starting");
            Mail();
           
        }

        private void Mail()
        {
            System.Configuration.ConfigurationManager.RefreshSection("mailSetting");//新加刷新获取最新配置邮件地址
            foreach (var mail in mailService.FetchMails())
            {
                if (ReplyFlag)
                {
                    string strExcelReply = MailExcelReply("\\邮件文言\\审核通知.txt");
                    mailService.ExcelReply(strExcelReply, mail.Subject + "--回执", mail.From);
                }
                //解析Excel存入到数据库
                AddRegistration(mail);
            }
        }

        static string MailExcelReply(string fileName)
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["mailTxt"];
            string strPath = path + fileName;
            FileStream fs = new FileStream(strPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string strExcelReply = sr.ReadToEnd();
            fs.Close();
            return strExcelReply;
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
                        Dictionary<string, object> dict = null;
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
                            AddLog(error, mail.From, 2, null, "计划任务");
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
                                string strPass = "申请表PDF不符合规范，发件人:" + mail.From + "\r\n";
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


        static void AddLog(string context, string mailUser, int type, int? recordId, string Operator)
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

      

        protected override void OnStop()
        {
            this.EventLog.WriteEntry("FetMailService stoping");
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

       

    }
}
