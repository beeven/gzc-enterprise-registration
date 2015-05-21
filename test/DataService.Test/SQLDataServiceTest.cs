using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using EnterpriseRegistration.DataService;
using EnterpriseRegistration.Interfaces.Entities;


namespace DataService.Test
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class SQLDataServiceTest
    {
        public SQLDataServiceTest()
        {
        }

        [Fact]
        public async void SaveTest()
        {
            SQLDataService svc = new SQLDataService();
            Message msg = new Message()
            {
                 From = "from@addr.com",
                 Body = "body",
                 Subject = "subject",
                 Attachments = new List<Attachment>()
                 {
                     new Attachment(){ 
                         FileName = "attachment.txt"
                     }
                 }
            };
            await svc.SaveAsync(msg);
            
            Assert.True(svc.DataContext.Count() >= 1);
            
            var msgId = msg.MessageId;
            Assert.NotEqual(new Guid(),msgId);
            
            var actual = await svc.GetByIdAsync(msgId);
            Assert.Equal(msg.From, actual.From);
            Assert.NotNull(actual.Attachments);
            Assert.Equal(1,actual.Attachments.Count);
            Assert.Equal("attachment.txt",actual.Attachments.First().FileName);
            Assert.Equal(actual.MessageId,actual.Attachments[0].MessageId);
        }
    }
}
