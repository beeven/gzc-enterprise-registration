using EnterpriseRegistration.Receipt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Receipt.Filters
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
