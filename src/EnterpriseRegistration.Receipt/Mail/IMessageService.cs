
using EnterpriseRegistration.Receipt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Receipt.Mail
{
    public interface IMessageService
    {
        IEnumerable<Message> GetMessages();

        Task SendMessage(String address, Message message);
    }
}
