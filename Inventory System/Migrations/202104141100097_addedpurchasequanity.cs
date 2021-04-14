namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedpurchasequanity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DemandItems", "PurchasedItemQuantity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DemandItems", "PurchasedItemQuantity");
        }
    }
}
