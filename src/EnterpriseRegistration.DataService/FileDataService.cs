using EnterpriseRegistration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseRegistration.Interfaces.Entities;
using Microsoft.Framework.ConfigurationModel;
using System.IO;

namespace EnterpriseRegistration.DataService
{
    public class FileDataService : IDataService
    {
        readonly Configuration conf;
        public FileDataService()
        {
            conf = new Configuration();
            conf.AddJsonFile("config.json");
        }

        public Task DeleteMessageAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Attachment> GetAttachmentByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetMessageByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Message> GetMessages(int pageSize, int offset)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Message> QueryMessagesAsync(Predicate<Message> predictate)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(Message message)
        {
            string basePath = conf.Get("Data:FileStorage:Path") ?? System.IO.Path.GetTempPath();
            
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            await Task.Run(() =>
            {
                Parallel.ForEach(message.Attachments, (att) =>
                {
                    string id = Guid.NewGuid().ToString("N").Substring(0, 10);
                    string path = $"{basePath}\\{id}_{message.FromAddress}_{message.DateSent.ToString("s").Replace(':','-')}_{att.FileName}";
                    File.WriteAllBytes(path, att.Content);
                });
            });


        }
    }
}
