namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemCategories",
                c => new
                    {
                        ItemCategoryId = c.Int(nullable: false, identity: true),
                        ItemCategoryName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ItemCategoryId);
            
            CreateTable(
                "dbo.ItemSubCategories",
                c => new
                    {
                        ItemSubCategoryId = c.Int(nullable: false, identity: true),
                        ItemSubCategoryName = c.String(),
                        ItemCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemSubCategoryId)
                .ForeignKey("dbo.ItemCategories", t => t.ItemCategoryId, cascadeDelete: true)
                .Index(t => t.ItemCategoryId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        ItemUnit = c.String(),
                        ItemQuantity = c.Double(nullable: false),
                        ItemAvgPrice = c.Double(nullable: false),
                        ItemSubCategoryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.ItemSubCategories", t => t.ItemSubCategoryId, cascadeDelete: true)
                .Index(t => t.ItemSubCategoryId);
            
            CreateTable(
                "dbo.ItemInputs",
                c => new
                    {
                        ItemInputId = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        ItemPrice = c.Double(nullable: false),
                        ItemQuantity = c.Int(nullable: false),
                        ItemTotalCost = c.Double(nullable: false),
                        ItemVendorId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Vendor_VendorId = c.Int(),
                    })
                .PrimaryKey(t => t.ItemInputId)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Vendors", t => t.Vendor_VendorId)
                .Index(t => t.ItemId)
                .Index(t => t.Vendor_VendorId);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        VendorId = c.Int(nullable: false, identity: true),
                        VendorName = c.String(),
                        VendorPhone = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VendorId);
            
            CreateTable(
                "dbo.ItemOutputs",
                c => new
                    {
                        ItemOutputId = c.Int(nullable: false, identity: true),
                        ItemOutputQuantity = c.Double(nullable: false),
                        ItemId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ItemOutputId)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            CreateTable(
                "dbo.ItemReturns",
                c => new
                    {
                        ItemReturnId = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        ItemQuantity = c.Double(nullable: false),
                        projectId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ItemReturnId)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.projectId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.projectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "ItemSubCategoryId", "dbo.ItemSubCategories");
            DropForeignKey("dbo.ItemReturns", "projectId", "dbo.Projects");
            DropForeignKey("dbo.ItemReturns", "ItemId", "dbo.Items");
            DropForeignKey("dbo.ItemOutputs", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ItemOutputs", "ItemId", "dbo.Items");
            DropForeignKey("dbo.ItemInputs", "Vendor_VendorId", "dbo.Vendors");
            DropForeignKey("dbo.ItemInputs", "ItemId", "dbo.Items");
            DropForeignKey("dbo.ItemSubCategories", "ItemCategoryId", "dbo.ItemCategories");
            DropIndex("dbo.ItemReturns", new[] { "projectId" });
            DropIndex("dbo.ItemReturns", new[] { "ItemId" });
            DropIndex("dbo.ItemOutputs", new[] { "ProjectId" });
            DropIndex("dbo.ItemOutputs", new[] { "ItemId" });
            DropIndex("dbo.ItemInputs", new[] { "Vendor_VendorId" });
            DropIndex("dbo.ItemInputs", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "ItemSubCategoryId" });
            DropIndex("dbo.ItemSubCategories", new[] { "ItemCategoryId" });
            DropTable("dbo.ItemReturns");
            DropTable("dbo.Projects");
            DropTable("dbo.ItemOutputs");
            DropTable("dbo.Vendors");
            DropTable("dbo.ItemInputs");
            DropTable("dbo.Items");
            DropTable("dbo.ItemSubCategories");
            DropTable("dbo.ItemCategories");
        }
    }
}
