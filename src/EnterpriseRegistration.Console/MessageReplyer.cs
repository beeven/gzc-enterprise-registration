using EnterpriseRegistration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseRegistration.DataService;
using System.Text.RegularExpressions;

namespace EnterpriseRegistration.Console
{
    public class MessageReplyer : IWorker
    {
        readonly ILogger logger;
        readonly IMessageService msgSvc;

        private MessageStoreContext ctx;
        public MessageReplyer(ILogger logger, IMessageService msgSvc)
        {
            this.logger = logger;
            ctx = new MessageStoreContext();
            this.msgSvc = msgSvc;

        }
        public async Task DoWork()
        {
            var files = ctx.RevertMails.Where(x => x.SendFlag == null || x.SendFlag == DataService.Models.SendStatus.Pending);
            foreach (var f in files)
            {
                var name_match = Regex.Match(f.FileName, @"^\w{10}_(.+@.+)_\d{4}-\d{2}-\d{2}T\d{2}-\d{2}-\d{2}_(.+)$");
                if (name_match.Success)
                {
                    string receipt = name_match.Groups[1].Value;
                    string filename = name_match.Groups[2].Value;
                    string subject = filename + (f.ErrorNum > 0 ? "解析错误" : "解析通过");
                    string body;
                    if (f.ErrorNum > 0)
                    {
                        body = $"文件 \"{filename}\" 解析错误。\n错误行数: {f.ErrorNum} \n正确行数: {f.RightNum}\n\n请启用模板文件的宏进行数据格式校验。";
                    }
                    else
                    {
                        body = $"文件 \"{filename}\" 解析通过。\n处理行数: {f.RightNum}";
                    }
                    Interfaces.Entities.Message msg = new Interfaces.Entities.Message()
                    {
                        Subject = subject,
                        Body = body
                    };
                    try
                    {
                        await msgSvc.SendMessage(receipt, msg);
                        f.SendFlag = DataService.Models.SendStatus.Sent;
                        logger.Log($"Replied message to {receipt} about {filename}.");
                    }
                    catch(Exception ex)
                    {
                        f.SendFlag = DataService.Models.SendStatus.Error;
                        logger.Log($"Error when reply message to {receipt} about {filename}.");
                        System.Console.WriteLine($"Message: {ex.Message}\r\nStackTrace: {ex.StackTrace}");
                    }
                }
            }

            await ctx.SaveChangesAsync();
        }
    }
}
