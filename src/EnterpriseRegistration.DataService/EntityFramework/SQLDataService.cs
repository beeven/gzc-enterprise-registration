using EnterpriseRegistration.Interfaces;
using Entities = EnterpriseRegistration.Interfaces.Entities;
using Models = EnterpriseRegistration.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseRegistration.DataService
{
    public class SQLDataService : IDataService
    {
        readonly MessageStoreContext db;
        public SQLDataService()
        {
            db = new MessageStoreContext();
        }
        public async Task SaveAsync(Entities.Message message)
        {
            Models.Message msg = new Models.Message()
            {
                Body = message.Body,
                Subject = message.Subject,
                DateSent = message.DateSent,
                FromAddress = message.FromAddress,
                FromName = message.FromName
            };

            foreach(var a in message.Attachments)
            {
                var file = new Models.AttachmentFile();
                file.file_stream = a.Content;
                file.name = Guid.NewGuid().ToString() + a.FileName.Substring(a.FileName.LastIndexOf("."));
                
                var attachment = new Models.Attachment()
                {
                    HashName = file.name,
                    File = file,
                    OriginalName = a.FileName,
                    MIMEType = a.MIMEType,
                    Size = a.Size == 0? a.Content.Length : a.Size,
                    Message = msg
                };

                msg.Attachments.Add(attachment);

                // EF7 beta4 doesn't save related data yet.
                db.AttachmentFiles.Add(file);
                db.Attachments.Add(attachment);
            }



            db.Messages.Add(msg);

            await db.SaveChangesAsync();

            message.MessageId = msg.MessageId;

            for(int i=0;i<msg.Attachments.Count;i++)
            {
                message.Attachments[i].AttachmentId = msg.Attachments[i].AttachmentId;
                message.Attachments[i].MessageId = msg.MessageId;
                message.Attachments[i].Message = message;
            }
        }

        public async Task<Entities.Message> GetMessageByIdAsync(Guid id)
        {
            var msg = await db.Messages.AsNoTracking().FirstAsync(x => x.MessageId == id);
            
            // EF7 beta4 doesn't load related data yet.
            msg.Attachments = await db.Attachments.Where(x => x.MessageId == id).ToListAsync();


            Entities.Message ret = new Entities.Message()
            {
                MessageId = msg.MessageId,
                Body = msg.Body,
                Subject = msg.Subject,
                FromAddress = msg.FromAddress,
                FromName = msg.FromName,
                DateSent = msg.DateSent
            };
            foreach(var a in msg.Attachments)
            {
                ret.Attachments.Add(new Entities.Attachment()
                {
                    FileName = a.OriginalName,
                    MIMEType = a.MIMEType,
                    Size = a.Size,
                    Message = ret,
                    MessageId = ret.MessageId,
                    AttachmentId = a.AttachmentId,
                    Content = null
                });
            }

            return ret;
        }

        public async Task DeleteMessageAsync(Guid id)
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

        public async Task<Entities.Attachment> GetAttachmentByIdAsync(Guid id)
        {
            var attachment = await db.Attachments.SingleOrDefaultAsync(x => x.AttachmentId == id);
            
            var file = await db.AttachmentFiles.SingleOrDefaultAsync(x => x.stream_id == attachment.stream_id);


            var ret = new Entities.Attachment()
            {
                AttachmentId = attachment.AttachmentId,
                Size = attachment.Size,
                FileName = attachment.OriginalName,
                MessageId = attachment.MessageId,
                MIMEType = attachment.MIMEType,
                Content = file.file_stream
            };

            return ret;
        }

        public IQueryable<Entities.Message> QueryMessagesAsync(System.Linq.Expressions.Expression<Func<Entities.Message,bool>> predictate)
        {
            
            throw new NotImplementedException();
        }

        public IQueryable<Entities.Message> QueryMessagesAsync(Predicate<Entities.Message> predictate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Entities.Message> GetMessages(int pageSize, int offset)
        {
            if(pageSize <= 0)
            {
                pageSize = 10;
            }
            List<Entities.Message> ret = new List<Entities.Message>();

            var msgs = db.Messages.OrderByDescending(x => x.DateSent).Skip(offset).Take(pageSize).ToList();

            var atts = from att in db.Attachments
                       where msgs.Select(x => x.MessageId).Contains(att.MessageId)
                       select new Entities.Attachment()
                       {
                           AttachmentId = att.AttachmentId,
                           FileName = att.OriginalName,
                           MessageId = att.MessageId,
                           MIMEType = att.MIMEType,
                           Size = att.Size,
                           Content = null
                       };
            ret = msgs.Select(x => new Entities.Message()
            {
                MessageId = x.MessageId,
                FromAddress = x.FromAddress,
                FromName = x.FromName,
                Subject = x.Subject,
                DateSent = x.DateSent,
                Body = x.Body,
                Attachments = atts.Where(a=>a.MessageId == x.MessageId).ToList()
            }).ToList();

            return ret.AsQueryable();
            
        }
    }
}