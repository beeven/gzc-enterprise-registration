using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseRegistration.Interfaces;
using EnterpriseRegistration.Interfaces.Entities;
#if !DNXCORE50
using MongoDB.Driver;
#endif

namespace EnterpriseRegistration.DataService
{
    public class MongoDataService : IDataService
    {

        public MongoDataService()
        {
           
        }


        public IQueryable<Message> Messages
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Message message)
        {
#if DNXCORE50
            throw new NotSupportedException();
#endif
            throw new NotImplementedException();
        }
    }
}
