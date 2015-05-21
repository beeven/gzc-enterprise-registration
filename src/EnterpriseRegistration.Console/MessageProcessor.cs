using System;
using System.Collections.Generic;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.Interfaces.Entities;

namespace EnterpriseRegistration.Console
{
	public class MessageProcessor
	{
		readonly IEnumerable<IMessageFilter> filters;
		readonly IMessageService msgService;
		readonly IDataService dataService;
		public MessageProcessor(IEnumerable<IMessageFilter> filters, IMessageService messageService, IDataService dataService)
		{
			this.filters = filters;
			this.msgService = messageService;
			this.dataService = dataService;
		}
		
		public void PopulateFilters()
		{
			foreach(var f in filters)
			{
				System.Console.WriteLine("Filter: {0}",f.GetType().ToString());
			}
		}
	}
}