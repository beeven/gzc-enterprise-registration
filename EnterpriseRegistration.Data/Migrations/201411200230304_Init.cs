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
                "dbo.CustomMaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MapCode = c.String(),
                        TopCustomCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomRights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomCode = c.String(),
                        AvailableCustomCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomName = c.String(),
                        CustomCode = c.String(),
                        Contact = c.String(),
                        Tel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        RecordNum = c.String(),
                        isRecordPass = c.Int(nullable: false),
                        isGetEntAccess = c.Int(nullable: false),
                        GetEntAccessDate = c.DateTime(),
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
                        UnitsFlag = c.Int(nullable: false),
                        Remark = c.String(),
                        RecordID = c.String(),
                        AttnMobile = c.String(),
                        AttnEmail = c.String(),
                        AttchID = c.Int(nullable: false),
                        ICPCode = c.String(),
                        OperateFlag = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SystemLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MailUser = c.String(),
                        LogContext = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "RegInfo_Id", "dbo.RegInfoes");
            DropIndex("dbo.Attachments", new[] { "RegInfo_Id" });
            DropTable("dbo.SystemLogs");
            DropTable("dbo.Registrations");
            DropTable("dbo.RegInfoes");
            DropTable("dbo.Customs");
            DropTable("dbo.CustomRights");
            DropTable("dbo.CustomMaps");
            DropTable("dbo.Attachments");
        }

    }
}
