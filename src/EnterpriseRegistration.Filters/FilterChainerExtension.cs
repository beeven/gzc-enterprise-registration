using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.Interfaces.Entities;

namespace EnterpriseRegistration.Filters
{
    public static class FilterChainerExtension
    {
        public static IEnumerable<Message> ApplyFilter(this IEnumerable<Message> messages, IMessageFilter filter)
        {
            return filter.Filter(messages);
        }

        public static IEnumerable<Message> ApplyFilter(this IEnumerable<Message> messages, IMessageFilter filter, Action<IEnumerable<Message>> actionOnNotQualified)
        {
            return filter.Filter(messages, actionOnNotQualified);
        }
    }
}
