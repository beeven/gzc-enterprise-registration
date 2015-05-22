using System;
using System.Collections.Generic;
using Autofac;
using EnterpriseRegistration.Interfaces;

namespace EnterpriseRegistration.Filters
{
	public class FiltersModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c=> new AttachmentFilter())
				.As<IMessageFilter>()
				.InstancePerDependency();
			
		}
	}
}