using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using EnterpriseRegistration.Interfaces.Entities;

namespace EnterpriseRegistration.MessageFilter
{
	public class AttachmentFilterTest: IDisposable
	{
		EnterpriseRegistration.Filters.AttachmentFilter target;
		List<Message> source;
		public AttachmentFilterTest()
		{
			target = new EnterpriseRegistration.Filters.AttachmentFilter();
			source = new List<Message>()
			{
				new Message(){
					FromAddress = "1",
					Attachments = new List<Attachment>(){}
				},
				new Message(){
					FromAddress = "2",
					Attachments = new List<Attachment>(){new Attachment(){FileName="0.txt"}}
				},
				new Message(){
					FromAddress = "3",
					Attachments = new List<Attachment>(){new Attachment(){FileName="1.xls"}}
				},
				new Message(){
					FromAddress = "4",
					Attachments = new List<Attachment>(){new Attachment(){FileName="2.xlsx"}}
				}
			};
		}
		
		public void Dispose()
		{
			source.Clear();
		}
		
		[Fact]
		public void AttachmentContainsXlsFiles_ShouldReturnTwo()
		{
			var actual = target.Filter(source);
			Assert.NotNull(actual);
			Assert.Equal(2, actual.Count());
		}
		
		[Fact]
		public void ShouldInvokeActionsOnNotQualified()
		{
			
			List<String> notQualified = new List<String>();
			var actual = target.Filter(source, x=>{
				notQualified.AddRange(x.Select(m=>m.FromAddress));
			});
			
			Assert.Equal(new String[]{"1","2"},notQualified);
			Assert.Equal(new String[]{"3","4"},actual.Select(x=>x.FromAddress));
		}
	}
}