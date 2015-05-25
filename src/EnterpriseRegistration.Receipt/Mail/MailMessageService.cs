using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;
using OpenPop.Pop3;
using System.Text.RegularExpressions;
using EnterpriseRegistration.Receipt.Models;
using EnterpriseRegistration.Receipt.Logging;

namespace EnterpriseRegistration.Receipt.Mail
{
    public class MailMessageService : IMessageService
    {
        readonly ILogger logger;
        Configuration conf;
        public MailMessageService(ILogger logger)
        {
            this.logger = logger;
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
                    logger.Log($"Connecting host: {host}, port: {port}");
                    client.Connect(host, port, false);
                    client.Authenticate(conf.Get("Mail:Incoming:Account"), conf.Get("Mail:Incoming:Password"));
                }
                catch(Exception ex)
                {
                    logger.Log($"Cannot connect to mail server. Exception: {ex.Message}\nStackTrace:{ex.StackTrace}");
                    yield break;
                }
                var infoes = client.GetMessageInfos();
                foreach(var info in infoes)
                {
                    logger.Log($"Mail #{info.Number}, Id: {info.Identifier}");

                    var m = client.GetMessage(info.Number);

                    logger.Log($"\tFrom:{m.Headers.From?.Address}");

                    Message msg = new Message()
                    {
                        Subject = m.Headers.Subject,
                        FromAddress = m.Headers.From.Address,
                        FromName = m.Headers.From.DisplayName,
                        DateSent = m.Headers.DateSent,
                    };
                    msg.Body = m.FindFirstHtmlVersion()?.GetBodyAsText();
                    if(msg.Body == null)
                    {
                        msg.Body = m.FindFirstPlainTextVersion().GetBodyAsText();
                    }
                    msg.Attachments = new List<Attachment>();
                    logger.Log("Attachments:");
                    foreach (var a in m.FindAllAttachments())
                    {
                        logger.Log($"\tFileName:{a.FileName}");
                        msg.Attachments.Add(new Attachment()
                        {
                            File = new AttachmentFile()
                            {
                                name = a.FileName,
                                file_stream = a.Body
                            }
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
            await client.SendMailAsync(message.FromAddress, address, message.Subject, message.Body);
           
        }
    }
}
