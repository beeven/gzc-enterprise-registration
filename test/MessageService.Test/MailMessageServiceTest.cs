using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using EnterpriseRegistration.MessageService;
using EnterpriseRegistration.Interfaces.Entities;

using Microsoft.Framework.ConfigurationModel;

namespace MessageService.Test
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class MailMessageServiceTest
    {
        MailMessageService target;
        public MailMessageServiceTest()
        {
            target = new MailMessageService(new ConsoleLogger());
        }

        [Fact]
        public void GetMails()
        {
            var actual = target.GetMessages();
            Assert.NotNull(actual);
            Console.WriteLine(actual.Count());
        }
    }
}
