using System;
using System.Collections.Generic;
using FetchMail.Models;

namespace FetchMail
{
	public interface IDataService
	{
		void StoreMessages(IEnumerable<Message> messages);
	}
}