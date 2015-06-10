using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using EnterpriseRegistration.DataService;
using EnterpriseRegistration.Interfaces.Entities;
using Microsoft.Framework.ConfigurationModel;
using FluentAssertions;
using System.IO;

namespace DataService.Test
{
    public class FileDataServiceTest:IDisposable
    {
        readonly FileDataService target;
        readonly Configuration conf;
        readonly string basePath;
        public FileDataServiceTest()
        {
            target = new FileDataService();
            conf = new Configuration();
            conf.AddJsonFile("config.json");
            basePath = conf.Get("Data:FileStorage:Path") ?? System.IO.Path.GetTempPath();
        }

        [Fact]
        public async Task Save_ShouldFindFiles()
        {
            Message msg = new Message()
            {
                FromAddress = "ABC@def.com",
                DateSent = DateTime.Parse("2015-06-10T11:00:00+08:00"),
                Attachments = new List<Attachment>()
                {
                    new Attachment() { FileName= "1.txt", Content= System.Text.Encoding.UTF8.GetBytes("Hello World!") },
                    new Attachment() { FileName= "2.txt", Content = System.Text.Encoding.UTF8.GetBytes("Hello again!") }
                }
            };

            await target.SaveAsync(msg);

            String[] files = Directory.GetFiles(basePath, "*ABC@def.com*.txt");
            files.Should().HaveCount(2);
            var file = files.Single(x=>x.EndsWith("1.txt"));
            File.ReadAllText(file, System.Text.Encoding.UTF8).Should().Be("Hello World!");
            
        }

        public void Dispose()
        {
            String[] files = Directory.GetFiles(basePath, "*ABC@def.com*.txt");
            foreach(var f in files)
            {
                File.Delete(f);
            }
        }
    }
}
