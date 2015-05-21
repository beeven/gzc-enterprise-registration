using EnterpriseRegistration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseRegistration.Interfaces.Entities;
using Microsoft.Framework.ConfigurationModel;
using OpenPop.Pop3;
using System.Text.RegularExpressions;

namespace EnterpriseRegistration.MessageService
{
    public class MailMessageService : IMessageService
    {
        readonly ILogger _logger;
        Configuration conf;
        public MailMessageService(ILogger logger)
        {
            _logger = logger;
            conf = new Configuration();
            conf.AddJsonFile("config.json");
            
        }

        public IEnumerable<Message> GetMessages()
        {
            string host = conf.Get("MailSetting:PopServer") ?? "mail.customs.gov.cn";
            string port = conf.Get("MailSetting:PopPort") ?? "110";

            using (var client = new Pop3Client())
            {
                try
                {
                    client.Connect(host, int.Parse(port), false);
                    client.Authenticate(conf.Get("MailSetting:Account"), conf.Get("MailSetting:Password"));
                }
                catch(Exception ex)
                {
                    _logger.Log(String.Format("Cannot connect to mail server. Exception: {0}\nStackTrace:{1}",ex.Message,ex.StackTrace));
                    yield break;
                }
                int count = client.GetMessageCount();
                for (int i = 0; i < count; i++)
                {
                    var m = client.GetMessage(i);
                    Message msg = new Message()
                    {
                        Subject = m.Headers.Subject,
                        From = m.Headers.From.Address,
                        DateReceived = m.Headers.DateSent,
                    };
                    msg.Body = m.FindFirstHtmlVersion()?.GetBodyAsText();
                    if(msg.Body == null)
                    {
                        msg.Body = m.FindFirstPlainTextVersion().GetBodyAsText();
                    }
                    msg.Attachments = new List<Attachment>();
                    foreach(var a in m.FindAllAttachments())
                    {
                        msg.Attachments.Add(new Attachment()
                        {
                            Content = a.Body,
                            FileName = a.FileName,
                            MIMEType = a.ContentType.MediaType
                        });
                    }

                    yield return msg;
                }

                if(Regex.IsMatch(conf.Get("MailSetting:DeleteAfterReceived"),"[Tt]rue|1|[Yy]es"))
                {
                    client.DeleteAllMessages();
                }
                
                client.Disconnect();
            }
               
        }

        public void SendMessage(string address, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
