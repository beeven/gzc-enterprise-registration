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
            conf.AddJsonFile("config.json")
                .AddUserSecrets(); // UserSecrets has higher priority
        }

        public IEnumerable<Message> GetMessages()
        {
            var confhost = conf.Get("Mail:Incoming:PopServer");
            var confport = conf.Get("Mail:Incoming:PopPort");
            string host = String.IsNullOrEmpty(confhost) ? "mail.customs.gov.cn" : confhost;
            int port;
            if (!int.TryParse(confport, out port)) port = 110;

            using (var client = new Pop3Client())
            {
                try
                {
                    _logger.Log($"host:{host}, port:{port}");
                    client.Connect(host, port, false);
                    client.Authenticate(conf.Get("Mail:Incoming:Account"), conf.Get("Mail:Incoming:Password"));
                }
                catch(Exception ex)
                {
                    _logger.Log($"Cannot connect to mail server. Exception: {ex.Message}\nStackTrace:{ex.StackTrace}");
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

                if(Regex.IsMatch(conf.Get("Mail:Incoming:DeleteAfterReceived"),"[Tt]rue|1|[Yy]es"))
                {
                    client.DeleteAllMessages();
                }
                
                client.Disconnect();
            }
               
        }

        public async Task SendMessage(string address, Message message)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = conf.Get("Mail:Outgoing:SmtpServer");
            client.Port = int.Parse("Mail:Outgoing:SmtpPort");
            var account = conf.Get("Mail:Outgoing:Account");
            var password = conf.Get("Mail:Outgoing:Password");
            client.Credentials = new System.Net.NetworkCredential(account,password);
            await client.SendMailAsync(message.From, address, message.Subject, message.Body);
           
        }
    }
}
