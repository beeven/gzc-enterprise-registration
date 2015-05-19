using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnterpriseRegistration.Data;

namespace EnterpriseRegistration.Data.Tests
{
    /// <summary>
    /// Summary description for TestMSSQLRegInfoService
    /// </summary>
    [TestClass]
    public class TestMSSQLRegInfoService
    {
        public TestMSSQLRegInfoService()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize() {
            
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() {
            
            Models.EntRegDb db = new Models.EntRegDb();
            db.Database.ExecuteSqlCommand("delete from dbo.RegInfoes; ");
        }
        //
        #endregion

        [TestMethod]
        public void TestSave()
        {
            //
            // TODO: Add test logic here
            //
            var target = new MSSQLRegInfoService();

            var expected = new Models.RegInfo()
            {
                From = "From@mail.com",
                Subject = "TestContent",
                IsReplied = false,
                ReceivedDate = new DateTime(2014,8,20),
                Attachments = new List<Models.Attachment>(){
                    new Models.Attachment() {
                        Id = new Guid("{E17263D4-EE56-4C35-B910-68F51B4B3B79}"),
                        MIME = "text/plain",
                        Size = 11,
                        Name = "test.txt"
                    }
                }
            };

            var actual = target.Save(expected);
            Assert.IsTrue(actual > 1);
        }

        [TestMethod]
        public void TestGet()
        {
            var target = new MSSQLRegInfoService();

            var expected = new Models.RegInfo()
            {
                Id = 1,
                From = "From@mail.com",
                Subject = "TestContent",
                IsReplied = false,
                ReceivedDate = new DateTime(2014, 8, 20),
                Attachments = new List<Models.Attachment>(){
                    new Models.Attachment() {
                        Id = new Guid("{E17263D4-EE56-4C35-B910-68F51B4B3B79}"),
                        MIME = "text/plain",
                        Size = 11,
                        Name = "test.txt"
                    }
                }
            };

            var actual = target.Get(1);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.From, actual.From);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var target = new MSSQLRegInfoService();

            var expected = new Models.RegInfo()
            {
                Id = 1,
                From = "Second@mail.com",
                Subject = "Modified content",
                IsReplied = false,
                ReceivedDate = new DateTime(2014, 8, 20),
                Attachments = new List<Models.Attachment>(){
                    new Models.Attachment() {
                        Id = new Guid("{E17263D4-EE56-4C35-B910-68F51B4B3B79}"),
                        MIME = "text/plain",
                        Size = 11,
                        Name = "test.txt"
                    }
                }
            };

            target.Update(expected);

            var actual = target.Get(1);
            Assert.AreEqual("Second@mail.com", actual.From);
            Assert.AreEqual("Modified content", actual.Subject);
        }

        [TestMethod]
        public void TestSetReplied()
        {
            var target = new MSSQLRegInfoService();

            target.SetReplied(1,"");

            var actual = target.Get(1);
            Assert.IsTrue(actual.IsReplied);
            Assert.IsNotNull(actual.RepliedDate);
        }
    }
}
