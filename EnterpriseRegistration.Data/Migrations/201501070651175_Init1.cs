namespace EnterpriseRegistration.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemLogs", "RecordID", c => c.Int());
            AddColumn("dbo.SystemLogs", "Operator", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemLogs", "Operator");
            DropColumn("dbo.SystemLogs", "RecordID");
        }
    }
}
