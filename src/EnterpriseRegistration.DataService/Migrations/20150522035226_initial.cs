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
            migration.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Body = table.Column(type: "nvarchar(max)", nullable: true),
                    DateReceived = table.Column(type: "datetime2", nullable: false),
                    From = table.Column(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column(type: "nvarchar(max)", nullable: true)
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
                    Content = table.Column(type: "varbinary(max)", nullable: true),
                    FileName = table.Column(type: "nvarchar(max)", nullable: true),
                    MIMEType = table.Column(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_Attachment_Message_MessageId",
                        columns: x => x.MessageId,
                        referencedTable: "Message",
                        referencedColumn: "MessageId");
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropSequence("DefaultSequence");
            migration.DropTable("Attachment");
            migration.DropTable("Message");
        }
    }
}
