using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.Interfaces.Entities;

namespace EnterpriseRegistration.Filters
{
    public class TrimAttachmentFilter : IMessageFilter
    {
        public IEnumerable<Message> Filter(IEnumerable<Message> source)
        {
            return Filter(source, null);
        }

        public IEnumerable<Message> Filter(IEnumerable<Message> source, Action<IEnumerable<Message>> actionOnNotQuaulified)
        {
            foreach(var msg in source)
            {
                msg.Attachments.RemoveAll(x=> !(x.FileName.ToLower().EndsWith("xlsm") || x.FileName.ToLower().EndsWith("xlsx")));   
            }
            return source;
        }
        
    }
}
