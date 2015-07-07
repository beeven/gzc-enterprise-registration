using EnterpriseRegistration.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace MessageFilter.Test
{
    public class TrimAttachmentFilterTest
    {
        EnterpriseRegistration.Filters.TrimAttachmentFilter target;
        List<Message> source;
        public TrimAttachmentFilterTest()
        {
            target = new EnterpriseRegistration.Filters.TrimAttachmentFilter();
            source = new List<Message>()
            {
                new Message(){
                    FromAddress = "1",
                    Attachments = new List<Attachment>(){ new Attachment() { FileName = "1.xlsx" }, new Attachment() { FileName = "2.xlsm" } }
                },
                new Message(){
                    FromAddress = "2",
                    Attachments = new List<Attachment>(){new Attachment(){FileName="0.txt"}, new Attachment() { FileName = "1.xlsx" } }
                },
                new Message(){
                    FromAddress = "3",
                    Attachments = new List<Attachment>(){new Attachment(){FileName="1.xls"}, new Attachment() { FileName = "2.xlsm" } }
                }
            };
        }

        public void Dispose()
        {
            source.Clear();
        }

        [Fact]
        public void AttachmentContainsXlsxAndXlsmFiles()
        {

            var actual = target.Filter(source);
            actual.Count().Should().Be(3);
            var m1 = actual.Single(x => x.FromAddress == "1");
            m1.Attachments.Should().HaveCount(2);
            m1.Attachments.Select(x => x.FileName).Should().Contain(new String[]{ "1.xlsx", "2.xlsm" });

            var m2 = actual.Single(x => x.FromAddress == "2");
            m2.Attachments.Select(x => x.FileName).Should().HaveCount(1).And.Contain(new String[] { "1.xlsx"  });

            var m3 = actual.Single(x => x.FromAddress == "3")
                        .Attachments
                        .Select(x => x.FileName)
                        .Should()
                        .ContainSingle()
                        .Which
                        .Should()
                        .Be("2.xlsm");
            
        }
    }
}
