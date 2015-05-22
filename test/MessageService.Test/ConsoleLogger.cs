using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;

namespace MessageService.Test
{
    public class ConsoleLogger : EnterpriseRegistration.Interfaces.ILogger
    {
        readonly ILogger logger;
        public ConsoleLogger()
        {
            LoggerFactory factory = new LoggerFactory();
            logger = factory.CreateLogger<MailMessageServiceTest>();
            factory.AddConsole();
        }
        public void Log(string content)
        {
            logger.LogInformation(content);
        }
    }
}
