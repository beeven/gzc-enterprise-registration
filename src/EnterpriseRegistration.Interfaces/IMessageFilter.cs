using EnterpriseRegistration.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Interfaces
{
    public interface IMessageFilter
    {
        IEnumerable<Message> Filter();

        IEnumerable<Message> Filter(Action<IEnumerable<Message>> actionOnNotquaulified);
    }
}
