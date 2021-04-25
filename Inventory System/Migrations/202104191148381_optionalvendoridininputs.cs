namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class optionalvendoridininputs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ItemInputs", "VendorId", "dbo.Vendors");
            DropIndex("dbo.ItemInputs", new[] { "VendorId" });
            AlterColumn("dbo.ItemInputs", "VendorId", c => c.Int());
            CreateIndex("dbo.ItemInputs", "VendorId");
            AddForeignKey("dbo.ItemInputs", "VendorId", "dbo.Vendors", "VendorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemInputs", "VendorId", "dbo.Vendors");
            DropIndex("dbo.ItemInputs", new[] { "VendorId" });
            AlterColumn("dbo.ItemInputs", "VendorId", c => c.Int(nullable: false));
            CreateIndex("dbo.ItemInputs", "VendorId");
            AddForeignKey("dbo.ItemInputs", "VendorId", "dbo.Vendors", "VendorId", cascadeDelete: true);
        }
    }
}
