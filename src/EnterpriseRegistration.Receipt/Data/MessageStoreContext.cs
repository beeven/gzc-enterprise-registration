using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using EnterpriseRegistration.Receipt.Models;
using Microsoft.Framework.ConfigurationModel;

namespace EnterpriseRegistration.Receipt.Data
{
    public class MessageStoreContext:DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<AttachmentFile> AttachmentFiles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Message>().Key(a => a.MessageId);

            builder.Entity<Attachment>().Key(a => a.AttachmentId);

            builder.Entity<AttachmentFile>().Key(a => a.stream_id);

            builder.Entity<Message>()
                .Collection(m => m.Attachments)
                .InverseReference(a => a.Message)
                .ForeignKey(a => a.MessageId);

            builder.Entity<Attachment>()
                .Reference(x => x.File);

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new Configuration();
            config.AddJsonFile("config.json");

            optionsBuilder.UseSqlServer(config.Get("Data:DefaultConnection:ConnectionString"));
            //optionsBuilder.UseInMemoryStore();

        }
    }
}
