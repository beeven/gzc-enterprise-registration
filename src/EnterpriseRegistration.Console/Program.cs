using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.MessageService;
using EnterpriseRegistration.DataService;
using EnterpriseRegistration.Filters;
using EnterpriseRegistration.Logging;

namespace EnterpriseRegistration.Console
{
    public class Program
    {
        IContainer container;
        public void Main(string[] args)
        {
            Configure();
            
            MessageProcessor processor = container.Resolve<MessageProcessor>();
            processor.DoWork();
            ILogger logger = container.Resolve<ILogger>();
            logger.Log("Job finished.")
        }
        
        
        void Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<MailMessageService>().As<IMessageService>();
            builder.RegisterType<SQLDataService>().As<IDataService>();
            builder.RegisterModule(new FiltersModule());
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<MessageProcessor>().AsSelf();
            
            container = builder.Build();
        }
        
    }
}
