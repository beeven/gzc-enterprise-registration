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
        readonly IEnumerable<IWorker> workers;

        public Program(IEnumerable<IWorker> workers)
        {
            this.workers = workers;
        }

        public void Main(string[] args)
        {
            Configure();

            Program p = container.Resolve<Program>();
            p.RunAllWorkers();

            ILogger logger = container.Resolve<ILogger>();
            logger.Log("Job finished.");
        }
        
        public void RunAllWorkers()
        {
            var handles = this.workers.Select(x => x.DoWork()).ToArray();
            Task.WaitAll(handles);
        }
        
        void Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<MailMessageService>().As<IMessageService>();
            builder.RegisterType<SQLDataService>().As<IDataService>();
            builder.RegisterType<FileDataService>().As<IDataService>();
            builder.RegisterModule(new FiltersModule());
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<MessageProcessor>().As<IWorker>();
            builder.RegisterType<MessageReplyer>().As<IWorker>();
            builder.RegisterType<Program>().AsSelf();
            container = builder.Build();
        }
        
    }
}
