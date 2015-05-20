using EnterpriseRegistration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseRegistration.Interfaces.Entities;

namespace EnterpriseRegistration.MessageService
{
    public class MailMessageService : IMessageService
    {
        public IEnumerable<Message> GetMessages()
        {
            throw new NotImplementedException();
            
        }

        public void SendMessage(string address, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
