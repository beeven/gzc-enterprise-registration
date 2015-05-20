using System;
using System.Collections.Generic;
using EnterpriseRegistration.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseRegistration.Interfaces.Entities;

namespace EnterpriseRegistration.DataService
{
	public class SQLDataService: IDataService
	{
		public void Save(Message message)
		{
			throw new NotImplementedException();
		}
		
		public Message Get(Guid id)
		{
			throw new NotImplementedException();
		}
		
		public void Delete(Guid id)
		{
			throw new NotImplementedException();
		}
		
		public IQueryable<Message> DataContext{get;set;}
	}
}