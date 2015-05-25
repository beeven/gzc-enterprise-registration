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
            get { return "20150525062040_initial"; }
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
                
                builder.Entity("EnterpriseRegistration.DataService.Models.Attachment", b =>
                    {
                        b.Property<Guid>("AttachmentId")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<string>("HashName")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<string>("MIMEType")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<Guid>("MessageId")
                            .Annotation("OriginalValueIndex", 3);
                        b.Property<string>("OriginalName")
                            .Annotation("OriginalValueIndex", 4);
                        b.Property<long>("Size")
                            .Annotation("OriginalValueIndex", 5);
                        b.Property<Guid>("stream_id")
                            .Annotation("OriginalValueIndex", 6);
                        b.Key("AttachmentId");
                    });
                
                builder.Entity("EnterpriseRegistration.DataService.Models.AttachmentFile", b =>
                    {
                        b.Property<DateTime>("creation_time")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<byte[]>("file_stream")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<bool>("is_archive")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<bool>("is_directory")
                            .Annotation("OriginalValueIndex", 3);
                        b.Property<bool>("is_hidden")
                            .Annotation("OriginalValueIndex", 4);
                        b.Property<bool>("is_offline")
                            .Annotation("OriginalValueIndex", 5);
                        b.Property<bool>("is_readonly")
                            .Annotation("OriginalValueIndex", 6);
                        b.Property<bool>("is_system")
                            .Annotation("OriginalValueIndex", 7);
                        b.Property<bool>("is_temporary")
                            .Annotation("OriginalValueIndex", 8);
                        b.Property<DateTime?>("last_access_time")
                            .Annotation("OriginalValueIndex", 9);
                        b.Property<DateTime>("last_write_time")
                            .Annotation("OriginalValueIndex", 10);
                        b.Property<string>("name")
                            .Annotation("OriginalValueIndex", 11);
                        b.Property<Guid>("stream_id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 12);
                        b.Key("stream_id");
                    });
                
                builder.Entity("EnterpriseRegistration.DataService.Models.Message", b =>
                    {
                        b.Property<string>("Body")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<DateTime>("DateSent")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<string>("FromAddress")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<string>("FromName")
                            .Annotation("OriginalValueIndex", 3);
                        b.Property<Guid>("MessageId")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 4);
                        b.Property<string>("Subject")
                            .Annotation("OriginalValueIndex", 5);
                        b.Key("MessageId");
                    });
                
                builder.Entity("EnterpriseRegistration.DataService.Models.Attachment", b =>
                    {
                        b.ForeignKey("EnterpriseRegistration.DataService.Models.Message", "MessageId");
                        b.ForeignKey("EnterpriseRegistration.DataService.Models.AttachmentFile", "stream_id");
                    });
                
                return builder.Model;
            }
        }
    }
}
