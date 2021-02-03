namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update7Database : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "ItemReminder", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "ItemReminder");
        }
    }
}
