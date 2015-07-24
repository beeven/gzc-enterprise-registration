using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace EnterpriseRegistration.DataService.Migrations
{
    public partial class initial : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateSequence(
                name: "DefaultSequence",
                type: "bigint",
                startWith: 1L,
                incrementBy: 10);
            //migration.CreateTable(
            //    name: "AttachmentFile",
            //    columns: table => new
            //    {
            //        creation_time = table.Column(type: "datetime2", nullable: false),
            //        file_stream = table.Column(type: "varbinary(max)", nullable: true),
            //        is_archive = table.Column(type: "bit", nullable: false),
            //        is_directory = table.Column(type: "bit", nullable: false),
            //        is_hidden = table.Column(type: "bit", nullable: false),
            //        is_offline = table.Column(type: "bit", nullable: false),
            //        is_readonly = table.Column(type: "bit", nullable: false),
            //        is_system = table.Column(type: "bit", nullable: false),
            //        is_temporary = table.Column(type: "bit", nullable: false),
            //        last_access_time = table.Column(type: "datetime2", nullable: true),
            //        last_write_time = table.Column(type: "datetime2", nullable: false),
            //        name = table.Column(type: "nvarchar(max)", nullable: true),
            //        stream_id = table.Column(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AttachmentFile", x => x.stream_id);
            //    });
            migration.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Body = table.Column(type: "nvarchar(max)", nullable: true),
                    DateSent = table.Column(type: "datetime2", nullable: false),
                    FromAddress = table.Column(type: "nvarchar(max)", nullable: true),
                    FromName = table.Column(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column(type:"datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageId);
                });
            migration.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    AttachmentId = table.Column(type: "uniqueidentifier", nullable: false),
                    HashName = table.Column(type: "nvarchar(max)", nullable: true),
                    MIMEType = table.Column(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column(type: "uniqueidentifier", nullable: false),
                    OriginalName = table.Column(type: "nvarchar(max)", nullable: true),
                    Size = table.Column(type: "bigint", nullable: false),
                    stream_id = table.Column(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_Attachment_Message_MessageId",
                        columns: x => x.MessageId,
                        referencedTable: "Message",
                        referencedColumn: "MessageId");
                    table.ForeignKey(
                        name: "FK_Attachment_AttachmentFile_stream_id",
                        columns: x => x.stream_id,
                        referencedTable: "AttachmentFile",
                        referencedColumn: "stream_id");
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropSequence("DefaultSequence");
            migration.DropTable("Attachment");
            //migration.DropTable("AttachmentFile");
            migration.DropTable("Message");
        }
    }
}
