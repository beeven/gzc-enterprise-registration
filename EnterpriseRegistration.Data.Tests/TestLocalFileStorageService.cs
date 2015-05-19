using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnterpriseRegistration.Data;

namespace EnterpriseRegistration.Data.Tests
{
    [TestClass]
    public class TestLocalFileStorageService
    {
        [TestMethod]
        public void TestUploadAndDownload()
        {
            String TestContent = "Test contents here.";
            LocalFileStorageService target = new LocalFileStorageService();
            Guid id = new Guid("{442AA60D-2188-463F-AE33-29722626BA2D}");
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(ms);
                sw.Write(TestContent);
                sw.Flush();
                ms.Seek(0, System.IO.SeekOrigin.Begin);

                
                id = target.Upload(ms);

                Assert.AreNotEqual(new Guid("{442AA60D-2188-463F-AE33-29722626BA2D}"),id);
            }
            



            using(System.IO.MemoryStream outStream = new System.IO.MemoryStream())
            {
                target.Download(outStream, id);
                System.IO.StreamReader sr = new System.IO.StreamReader(outStream);
                outStream.Seek(0, System.IO.SeekOrigin.Begin);
                var actual = sr.ReadToEnd();

                Assert.AreEqual(TestContent, actual);
            }
            

            target.Delete(id);
        }
    }
}
