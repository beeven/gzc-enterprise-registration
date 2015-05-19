using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Framework.ConfigurationModel;

namespace FetchMail
{
    public class Program
    {
		private static IContainer Container{get;set;}
		public Task<int> Main(string[] args)
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<PopMailService>().As<IMailService>();
			
			Container = builder.Build();
			
			var conf = new Configuration()
					.AddJsonFile("config.json")
					.AddEnvironmentVariables();
			
			Console.WriteLine("Hello World");
			System.Console.WriteLine("Conf: \n\tPopServer: {0}",conf.Get("MailService:PopServer"));
			return Task.FromResult(0);
		}
    }
}