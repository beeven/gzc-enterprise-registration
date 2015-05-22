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

            // EF7 beta4 doesn't save related data yet.
            db.Attachments.AddRange(message.Attachments);

            await db.SaveChangesAsync();
        }

        public async Task<Message> GetByIdAsync(Guid id)
        {
            var ret = await db.Messages.AsNoTracking().FirstAsync(x => x.MessageId == id);
            
            // EF7 beta4 doesn't load related data yet.
            ret.Attachments = await db.Attachments.Where(x => x.MessageId == id).ToListAsync();

            return ret;
        }

        public async Task DeleteAsync(Guid id)
        {
            // EF7 doesn't support cascade delete on foreign key yet.
            db.Attachments.RemoveRange(db.Attachments.Where(x => x.MessageId == id));
            db.Messages.Remove(await db.Messages.SingleOrDefaultAsync(x => x.MessageId == id));
            await db.SaveChangesAsync();

            //var itemToRemove = await db.Messages.SingleOrDefaultAsync(x => x.MessageId == id);
            //if (itemToRemove != null)
            //{
            //    db.Messages.Remove(itemToRemove);
            //    await db.SaveChangesAsync();
            //}
        }

        public IQueryable<Message> Messages
        {
            get
            {
                return db.Messages.AsQueryable();
            }
        }
    }
}