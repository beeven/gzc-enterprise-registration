using EnterpriseRegistration.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Interfaces
{
    public interface IDataService
    {
        Task SaveAsync(Message message);
        Task<Message> GetMessageByIdAsync(Guid id);

        Task<Attachment> GetAttachmentByIdAsync(Guid id);

        Task DeleteMessageAsync(Guid id);


        IQueryable<Message> QueryMessagesAsync(Predicate<Message> predictate);

        IQueryable<Message> GetMessages(int pageSize, int offset);
    }
}
