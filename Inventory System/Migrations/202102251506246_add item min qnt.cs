namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additemminqnt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "ItemMinQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "ItemMinQuantity");
        }
    }
}
