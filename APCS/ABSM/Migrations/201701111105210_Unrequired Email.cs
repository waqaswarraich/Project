namespace ABSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnrequiredEmail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GeneralComplains", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GeneralComplains", "Email", c => c.String(nullable: false));
        }
    }
}
