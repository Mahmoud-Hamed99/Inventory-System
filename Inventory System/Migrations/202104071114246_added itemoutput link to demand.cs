namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeditemoutputlinktodemand : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DemandItems", "ItemId", "dbo.Items");
            DropIndex("dbo.DemandItems", new[] { "ItemId" });
            RenameColumn(table: "dbo.DemandItems", name: "ItemId", newName: "Item_ItemId");
            AddColumn("dbo.DemandItems", "ItemOutputId", c => c.Int(nullable: false));
            AlterColumn("dbo.DemandItems", "Item_ItemId", c => c.Int());
            CreateIndex("dbo.DemandItems", "ItemOutputId");
            CreateIndex("dbo.DemandItems", "Item_ItemId");
            AddForeignKey("dbo.DemandItems", "ItemOutputId", "dbo.ItemOutputs", "ItemOutputId", cascadeDelete: true);
            AddForeignKey("dbo.DemandItems", "Item_ItemId", "dbo.Items", "ItemId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DemandItems", "Item_ItemId", "dbo.Items");
            DropForeignKey("dbo.DemandItems", "ItemOutputId", "dbo.ItemOutputs");
            DropIndex("dbo.DemandItems", new[] { "Item_ItemId" });
            DropIndex("dbo.DemandItems", new[] { "ItemOutputId" });
            AlterColumn("dbo.DemandItems", "Item_ItemId", c => c.Int(nullable: false));
            DropColumn("dbo.DemandItems", "ItemOutputId");
            RenameColumn(table: "dbo.DemandItems", name: "Item_ItemId", newName: "ItemId");
            CreateIndex("dbo.DemandItems", "ItemId");
            AddForeignKey("dbo.DemandItems", "ItemId", "dbo.Items", "ItemId", cascadeDelete: true);
        }
    }
}
