using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnterpriseRegistration.Mail.Tests
{
    [TestClass]
    public class TestConfiguration
    {
        [TestMethod]
        public void TestMailConfiguration()
        {
            var actual = System.Configuration.ConfigurationManager.GetSection("mailSetting") as EnterpriseRegistration.Mail.MailConfiguration;
            Assert.AreEqual("172.7.1.2", actual.PopServer);
            Assert.AreEqual("172.7.1.2", actual.SmtpServer);
            Assert.AreEqual(110, actual.PopPort);
           
        }
    }
}
