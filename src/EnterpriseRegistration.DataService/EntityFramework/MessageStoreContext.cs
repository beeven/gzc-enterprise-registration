using EnterpriseRegistration.Interfaces.Entities;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;

namespace EnterpriseRegistration.DataService
{
    public class MessageStoreContext: DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Message>().Key(a => a.MessageId);

            builder.Entity<Message>()
                .Collection(m => m.Attachments)
                .InverseReference(a => a.Message)
                .ForeignKey(a => a.MessageId);
            
            //builder.Entity<Attachment>().Key(a=>a.AttachmentId);
            //builder.Entity<Message>().Property(a => a.MessageId).ForSqlServer(b => b.UseSequence());
            //builder.Entity<Attachment>().Property(a => a.AttachmentId).ForSqlServer(b => b.UseSequence());
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
