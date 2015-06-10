using System;
using System.Collections.Generic;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.Interfaces.Entities;
using EnterpriseRegistration.Filters;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Console
{
	public class MessageProcessor
	{
		readonly IEnumerable<IMessageFilter> filters;
		readonly IMessageService msgService;
		readonly IEnumerable<IDataService> dataServices;
		readonly ILogger logger;
		public MessageProcessor(IEnumerable<IMessageFilter> filters, 
								IMessageService messageService, 
								IEnumerable<IDataService> dataServices,
								ILogger logger)
		{
			this.filters = filters;
			this.msgService = messageService;
			this.dataServices = dataServices;
			this.logger = logger;
		}

        public async Task DoWork()
        {
            Message msgReplySuccess = new Message()
            {
                Subject = "您的邮件已经收到",
                Body = "<div>您的邮件已经收到</div>"
            };
			logger.Log("Entering DoWork...");
            var result = msgService.GetMessages();
			
            foreach(var f in filters)
            {
                result = result.ApplyFilter(f,elems=>{
					foreach(var elem in elems) {
						logger.Log($"Not qualified: {elem.FromAddress}");
					}
				});
            }
            foreach(var r in result)
            {
                foreach(var svc in dataServices)
                {
                    await svc.SaveAsync(r);
                }
                
            }
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