using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using EnterpriseRegistration.DataService;

namespace DataService.Test
{
    public class MessageStoreContextTest
    {

        MessageStoreContext target;

        public MessageStoreContextTest()
        {
            target = new MessageStoreContext();
        }

        [Fact]
        public void Context_ShouldHaveRevertMailTable()
        {
            target.RevertMails.ToArray().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ContextRevertmailTable_ShouldHaveKeyOID()
        {
            //var actual = target.RevertMails.GroupBy(x=>x.OID,x=>x.FileName).Count();
            //var expected = target.RevertMails.Count();
            //actual.Should().Be(expected);

            var actual = from m in target.RevertMails
                         group m by m.OID into g
                         select g;
            actual.Count().Should().BeGreaterThan(0);


            var obj1 = new EnterpriseRegistration.DataService.Models.RevertMail()
            {
                FileName = "1234567890_beeven@hotmail.com_2015-07-01T17-45-55_hello.txt",
                RightNum = 123,
                ErrorNum = 456,
                SendFlag = 0
            };

            var obj2 = new EnterpriseRegistration.DataService.Models.RevertMail()
            {
                FileName = "1234567890_beeven@hotmail.com_2015-07-01T17-45-55_hello.txt",
                RightNum = 123,
                ErrorNum = 456,
                SendFlag = 0
            };

            target.RevertMails.AddRange(obj1, obj2);
            target.SaveChanges();
            obj1.OID.Should().NotBe(obj2.OID);
        }
    }
}
