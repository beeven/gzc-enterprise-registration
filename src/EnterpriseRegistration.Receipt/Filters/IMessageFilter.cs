using System;
using System.Collections.Generic;
using EnterpriseRegistration.Receipt.Models;

namespace EnterpriseRegistration.Receipt.Filters
{
    public interface IMessageFilter
    {
        IEnumerable<Message> Filter(IEnumerable<Message> source);

        IEnumerable<Message> Filter(IEnumerable<Message> source, Action<IEnumerable<Message>> actionOnNotQualified);

    }
}