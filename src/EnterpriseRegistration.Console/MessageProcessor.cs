using System;
using System.Collections.Generic;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.Interfaces.Entities;
using EnterpriseRegistration.Filters;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;

namespace EnterpriseRegistration.Console
{
	public class MessageProcessor
	{
		readonly IEnumerable<IMessageFilter> filters;
		readonly IMessageService msgService;
		readonly IEnumerable<IDataService> dataServices;
		readonly ILogger logger;
        readonly Configuration conf;
        private bool replyMsg = false;
        private Message msgReplySuccess;
		public MessageProcessor(IEnumerable<IMessageFilter> filters, 
								IMessageService messageService, 
								IEnumerable<IDataService> dataServices,
								ILogger logger)
		{
			this.filters = filters;
			this.msgService = messageService;
			this.dataServices = dataServices;
			this.logger = logger;
            conf = new Configuration();
            conf.AddJsonFile("config.json");
            replyMsg = conf.Get("Mail:Reply:DoReply");
            if (replyMsg)
            {
                msgReplySuccess = new Message()
                {
                    Subject = conf.Get("Mail:Reply:Subject"),
                    Body = conf.Get("Mail:Reply:Body")
                };
            }
		}

        public async Task DoWork()
        {
            
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

                if(replyMsg)
                {
                    await msgService.SendMessage(r.FromAddress, msgReplySuccess);
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