using System;
using System.Collections.Generic;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.Interfaces.Entities;
using EnterpriseRegistration.Filters;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;
using System.Linq;

namespace EnterpriseRegistration.Console
{
	public class MessageProcessor:IWorker
	{
		readonly IEnumerable<IMessageFilter> filters;
		readonly IMessageService msgService;
		readonly IEnumerable<IDataService> dataServices;
		readonly IDataService fileDataService;
		readonly IDataService sqlDataService;
		readonly ILogger logger;
        readonly Configuration conf;
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
            
            
		}

        public async Task DoWork()
        {
                logger.Log("Fetching mails...");         
            var result = msgService.GetMessages();
			
			List<String> invalidMessageAddresses = new List<String>();
			
            foreach(var f in filters)
            {
                logger.Log($"Applying Filter: {f.GetType().Name}");
                result = result.ApplyFilter(f,elems=>{
					foreach(var elem in elems) {

						logger.Log($"Not qualified: {elem.FromAddress}. Attachments: {String.Join(",",elem.Attachments.Select(x=>x.FileName))}");

                        invalidMessageAddresses.Add(elem.FromAddress);
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
			foreach(var addr in invalidMessageAddresses)
			{
				await msgService.SendMessage(addr,new Message(){
                    Subject = "附件内容不合法",
                    Body = "附件内容不符合校验规则，请下载最新模板填写后再次发送。\n" +
                            "http://211.155.17.204/entreg/pay.rar"
                });
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