using System;

namespace FetchMail.Models
{
	public class Attachment
	{
		public byte[] Content {get; set;}
		
		public String FileName {get; set;}
		
		public String MimeType {get; set;}
	}
}