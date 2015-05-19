using System;
using System.Collections.Generic;
using FetchMail.Models;

namespace FetchMail
{
	public interface IMailService
	{
		IEnumerable<Message> GetMessages();
	}
}