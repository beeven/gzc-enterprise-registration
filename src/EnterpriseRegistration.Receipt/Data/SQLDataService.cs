using EnterpriseRegistration.Receipt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Receipt.Data
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
            // EF7 beta4 doesn't save related data yet.
            foreach (var a in message.Attachments)
            {
                db.AttachmentFiles.Add(a.File);
                //db.Attachments.Add(file);
            }

            db.Messages.Add(message);

            foreach(var a in message.Attachments)
            {
                db.Attachments.Add(a);
            }
            


            await db.SaveChangesAsync();
        }

        public async Task<Message> GetByIdAsync(Guid id)
        {
            var ret = await db.Messages.AsNoTracking().FirstAsync(x => x.MessageId == id);
            
            // EF7 beta4 doesn't load related data yet.
            ret.Attachments = await db.Attachments.Where(x => x.MessageId == id).ToListAsync();
            
            foreach(var a in ret.Attachments)
            {
                a.File = await db.AttachmentFiles.SingleOrDefaultAsync(x => x.stream_id == a.stream_id);
            }

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