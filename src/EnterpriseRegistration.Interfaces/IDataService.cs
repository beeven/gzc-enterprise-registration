using EnterpriseRegistration.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Interfaces
{
    public interface IDataService
    {
        void Save(Message message);
        Message Get(Guid id);

        void Delete(Guid id);

        IQueryable<Message> DataContext { get; }
    }
}
