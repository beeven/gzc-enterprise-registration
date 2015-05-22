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
        Task<Message> GetByIdAsync(Guid id);

        Task DeleteAsync(Guid id);

        IQueryable<Message> Messages { get; }
    }
}
