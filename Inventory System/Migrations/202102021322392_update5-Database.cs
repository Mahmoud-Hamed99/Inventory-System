namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update5Database : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "ItemQuantityAdded", c => c.Double(nullable: false));
            AddColumn("dbo.Items", "ItemQuantityWithdraw", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "ItemQuantityWithdraw");
            DropColumn("dbo.Items", "ItemQuantityAdded");
        }
    }
}
