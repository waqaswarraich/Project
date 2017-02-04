namespace ABSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StatusDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shops", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.GeneralComplains", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneralComplains", "Status");
            DropColumn("dbo.Shops", "Date");
        }
    }
}
