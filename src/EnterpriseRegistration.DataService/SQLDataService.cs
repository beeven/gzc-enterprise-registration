using System;
using System.Collections.Generic;
using EnterpriseRegistration.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseRegistration.Interfaces.Entities;

namespace EnterpriseRegistration.DataService
{
    public class SQLDataService : IDataService
    {
        readonly MessageStoreContext db;
        public SQLDataService()
        {
            db = new MessageStoreContext();
        }
        public async Task SaveAsync(Message message)
        {
            db.Messages.Add(message);
            await db.SaveChangesAsync();
        }

        public async Task<Message> GetByIdAsync(Guid id)
        {
            return await db.Messages.FirstAsync(x => x.MessageId == id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var itemToRemove = db.Messages.SingleOrDefault(x => x.MessageId == id);
            if (itemToRemove != null)
            {
                db.Messages.Remove(itemToRemove);
                await db.SaveChangesAsync();
            }
        }

        public IQueryable<Message> DataContext
        {
            get
            {
                return db.Messages.AsQueryable();
            }
        }
    }
}