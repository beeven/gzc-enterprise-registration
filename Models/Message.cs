using System;
using System.Collections.Generic;

namespace FetchMail.Models
{
	public class Message
	{
		public String From{get;set;}
		
		public String Subject {get;set;}
		
		public String Body {get;set;}
		
		public DateTime DateReceived {get;set;}
		
		public IEnumerable<Attachment> Attachments {get;set;}
	}
}