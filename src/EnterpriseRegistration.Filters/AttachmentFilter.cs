using System;
using System.Linq;
using System.Collections.Generic;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.Interfaces.Entities;

namespace EnterpriseRegistration.Filters
{
	///<summary>
	/// Filter attachments contains xls[x] files
	///</summary>
	public class AttachmentFilter:IMessageFilter
	{
		public IEnumerable<Message> Filter(IEnumerable<Message> source)
		{
			return Filter(source, null);
		}
		
		public IEnumerable<Message> Filter(IEnumerable<Message> source, Action<IEnumerable<Message>> actionOnNotQualified)
		{
			var result = source.Where(x=>
				x.Attachments.Any(a=> a.FileName.EndsWith("xls") || a.FileName.EndsWith("xlsx"))
			);
			if(actionOnNotQualified != null)
				actionOnNotQualified.Invoke(source.Except(result));
			return result;
		}
	}
}