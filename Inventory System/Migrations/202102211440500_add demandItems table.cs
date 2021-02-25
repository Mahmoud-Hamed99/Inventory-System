namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddemandItemstable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DemandItems",
                c => new
                    {
                        DemandItemId = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        DemandItemQuantity = c.Double(nullable: false),
                        DemandItemPriority = c.Int(nullable: false),
                        DemandItemApproval = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DemandItemId)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DemandItems", "ItemId", "dbo.Items");
            DropIndex("dbo.DemandItems", new[] { "ItemId" });
            DropTable("dbo.DemandItems");
        }
    }
}
