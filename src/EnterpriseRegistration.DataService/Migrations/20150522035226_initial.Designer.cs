using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using EnterpriseRegistration.DataService;

namespace EnterpriseRegistration.DataService.Migrations
{
    [ContextType(typeof(MessageStoreContext))]
    partial class initial
    {
        public override string Id
        {
            get { return "20150522035226_initial"; }
        }
        
        public override string ProductVersion
        {
            get { return "7.0.0-beta4-12943"; }
        }
        
        public override IModel Target
        {
            get
            {
                var builder = new BasicModelBuilder()
                    .Annotation("SqlServer:ValueGeneration", "Sequence");
                
                builder.Entity("EnterpriseRegistration.Interfaces.Entities.Attachment", b =>
                    {
                        b.Property<Guid>("AttachmentId")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<byte[]>("Content")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<string>("FileName")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<string>("MIMEType")
                            .Annotation("OriginalValueIndex", 3);
                        b.Property<Guid>("MessageId")
                            .Annotation("OriginalValueIndex", 4);
                        b.Key("AttachmentId");
                    });
                
                builder.Entity("EnterpriseRegistration.Interfaces.Entities.Message", b =>
                    {
                        b.Property<string>("Body")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<DateTime>("DateReceived")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<string>("From")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<Guid>("MessageId")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 3);
                        b.Property<string>("Subject")
                            .Annotation("OriginalValueIndex", 4);
                        b.Key("MessageId");
                    });
                
                builder.Entity("EnterpriseRegistration.Interfaces.Entities.Attachment", b =>
                    {
                        b.ForeignKey("EnterpriseRegistration.Interfaces.Entities.Message", "MessageId");
                    });
                
                return builder.Model;
            }
        }
    }
}
