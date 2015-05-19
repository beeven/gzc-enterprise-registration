namespace EnterpriseRegistration.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
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
            
            CreateTable(
                "dbo.Registrations",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        EnterPriseName = c.String(),
                        EnterPriseAbbreviation = c.String(),
                        CustomsName = c.String(),
                        CustomsID = c.String(),
                        OnlineFlag = c.Int(nullable: false),
                        DirectFlag = c.Int(nullable: false),
                        RetailFlag = c.Int(nullable: false),
                        ManagementFlag = c.Int(nullable: false),
                        TransactionFlag = c.Int(nullable: false),
                        LogisticsFlag = c.Int(nullable: false),
                        PayFlag = c.Int(nullable: false),
                        Abroad = c.Int(nullable: false),
                        OrgCode = c.String(),
                        LegalName = c.String(),
                        LegalID = c.String(),
                        LicenseID = c.String(),
                        TaxCode = c.String(),
                        EnterPrisePhone = c.String(),
                        EnterPriseContact = c.String(),
                        EnterPriseAddress = c.String(),
                        TransactionName = c.String(),
                        TransactionWebsite = c.String(),
                        LogisticsWebsite = c.String(),
                        LegalCopyFlag = c.Int(nullable: false),
                        TaxCopyFlag = c.Int(nullable: false),
                        OrgCopyFlag = c.Int(nullable: false),
                        PayCopyFlag = c.Int(nullable: false),
                        LicenseCopyFlag = c.Int(nullable: false),
                        Remark = c.String(),
                        AttchID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "RegInfo_Id", "dbo.RegInfoes");
            DropIndex("dbo.Attachments", new[] { "RegInfo_Id" });
            DropTable("dbo.Registrations");
            DropTable("dbo.RegInfoes");
            DropTable("dbo.Attachments");
        }
    }
}
