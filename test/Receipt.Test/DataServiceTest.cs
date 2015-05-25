using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using EnterpriseRegistration.Receipt;
using EnterpriseRegistration.Receipt.Models;
using EnterpriseRegistration.Receipt.Data;
using FluentAssertions;

namespace Receipt.Test
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class DataServiceTest
    {
        IDataService target;
        public DataServiceTest()
        {
            target = new SQLDataService();
        }

        [Fact]
        public async void SaveAndGet()
        {
            Message msg = new Message()
            {
                FromName = "Renee Benitty",
                FromAddress = "renee.benitty@readify.net",
                Subject = "Subject",
                Body = "Body",
                DateSent = DateTime.Now
            };

            AttachmentFile file = new AttachmentFile()
            {
                name = "attachment.txt",
                file_stream = System.Text.Encoding.UTF8.GetBytes("This is the content.")
            };

            msg.Attachments.Add(new Attachment() { File = file });

            await target.SaveAsync(msg);

            var msgId = msg.MessageId;

            var actual = await target.GetByIdAsync(msgId);

            actual.Should().NotBeNull();
            actual.FromName.Should().Be("Renee Benitty");
            actual.Attachments.Should().NotBeNullOrEmpty();
            actual.Attachments.Should().HaveCount(1);
            actual.Attachments[0].File.Should().NotBeNull();
            actual.Attachments[0].File.name.Should().Be("attachment.txt");
            System.Text.Encoding.UTF8.GetString(actual.Attachments[0].File.file_stream).Should().Be("This is the content.");

            //Assert.NotNull(actual);
            //Assert.Equal("Renee Benitty", actual.FromName);
            //Assert.NotNull(actual.Attachments);
            //Assert.Equal(1, actual.Attachments.Count);
            //Assert.NotNull(actual.Attachments[0].File);
            //Assert.Equal("attachment.txt", actual.Attachments[0].File.name);
            //Assert.Equal("This is the content.", System.Text.Encoding.UTF8.GetString(actual.Attachments[0].File.file_stream));
        }
    }
}
