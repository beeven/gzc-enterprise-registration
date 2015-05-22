using EnterpriseRegistration.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Interfaces
{
    public interface IMessageService
    {
        IEnumerable<Message> GetMessages();

        Task SendMessage(String address, Message message);
    }
}
