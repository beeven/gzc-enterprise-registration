using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnterpriseRegistration.Mail.Tests
{
    [TestClass]
    public class TestMailService
    {
        [TestMethod]
        public void TestFetchMail()
        {
            var target = new MailService();

            var actual = target.FetchMails();

            Assert.IsTrue(actual.Count > 0);
        }
    }
}
