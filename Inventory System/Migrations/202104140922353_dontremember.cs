namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dontremember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLogs", "Username", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLogs", "Username");
        }
    }
}
