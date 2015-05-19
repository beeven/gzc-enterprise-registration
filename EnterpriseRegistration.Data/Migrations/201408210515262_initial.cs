namespace EnterpriseRegistration.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        MIME = c.String(),
                        Size = c.Long(nullable: false),
                        RegInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RegInfoes", t => t.RegInfo_Id)
                .Index(t => t.RegInfo_Id);
            
            CreateTable(
                "dbo.RegInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        From = c.String(),
                        Body = c.String(),
                        ReceivedDate = c.DateTime(nullable: false),
                        IsReplied = c.Boolean(nullable: false),
                        RepliedDate = c.DateTime(),
                        RepliedBody = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "RegInfo_Id", "dbo.RegInfoes");
            DropIndex("dbo.Attachments", new[] { "RegInfo_Id" });
            DropTable("dbo.RegInfoes");
            DropTable("dbo.Attachments");
        }
    }
}
