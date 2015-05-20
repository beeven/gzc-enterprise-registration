using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.MessageService;
using EnterpriseRegistration.DataService;
using EnterpriseRegistration.Filters;

namespace EnterpriseRegistration.Console
{
    public class Program
    {
        IContainer container;
        public void Main(string[] args)
        {
            
        }
        
        
        void Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<MailMessageService>().As<IMessageService>();
            builder.RegisterType<SQLDataService>().As<IDataService>();
            builder.RegisterModule(new FiltersModule());
            
            container = builder.Build();
        }
        
    }
}
