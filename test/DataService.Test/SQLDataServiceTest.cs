using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using EnterpriseRegistration.DataService;
using EnterpriseRegistration.Interfaces.Entities;
using FluentAssertions;


namespace DataService.Test
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class SQLDataServiceTest
    {

        readonly SQLDataService target;
        public SQLDataServiceTest()
        {
           target  = new SQLDataService();
        }

        [Fact]
        public async void SaveAndGetMessage()
        {

            string stringContent = "This is the content.";
            byte[] content = System.Text.Encoding.UTF8.GetBytes(stringContent);
            int sizeOfContent = System.Text.Encoding.UTF8.GetByteCount(stringContent);

            Message msg = new Message()
            {
                 FromAddress = "from@addr.com",
                 FromName = "Renee Benitty",
                 Body = "body",
                 Subject = "subject",
                 DateSent = DateTime.Now,
                 Attachments = new List<Attachment>()
                 {
                     new Attachment(){ 
                         FileName = "attachment.txt",
                         MIMEType = "text/plain",
                         Content = content,
                         Size = 0 // should calculate automatically if zero
                     }
                 }
            };

            await target.SaveAsync(msg);

            var msgId = msg.MessageId;
            msgId.Should().NotBeEmpty();

            var actual = await target.GetMessageByIdAsync(msgId);
            actual.Should().NotBeNull();
            actual.Should().NotBeSameAs(msg);
            actual.FromAddress.Should().Be("from@addr.com");
            actual.FromName.Should().Be("Renee Benitty");
            actual.Body.Should().Be("body");
            actual.Subject.Should().Be("subject");
            actual.Attachments.Should().NotBeNullOrEmpty().And.HaveCount(1);

            var attachmentInfo = actual.Attachments.ElementAt(0);
            attachmentInfo.AttachmentId.Should().NotBeEmpty();
            attachmentInfo.FileName.Should().Be("attachment.txt");
            attachmentInfo.MIMEType.Should().Be("text/plain");
            attachmentInfo.MessageId.Should().Be(msgId);
            attachmentInfo.Message.Should().BeSameAs(actual);

            // according to the interface documentation
            attachmentInfo.Content.Should().BeNull("it is not necessory to load all content into memory");

            var attachment = await target.GetAttachmentByIdAsync(attachmentInfo.AttachmentId);
            System.Text.Encoding.UTF8.GetString(attachment.Content).Should().Be("This is the content.");
            attachment.Size.Should().Be(sizeOfContent);
            
        }
        

    }
}
