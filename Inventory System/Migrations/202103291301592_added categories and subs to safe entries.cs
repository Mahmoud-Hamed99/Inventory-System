namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcategoriesandsubstosafeentries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SafeSubCategories",
                c => new
                    {
                        SafeSubCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        SafeCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SafeSubCategoryId)
                .ForeignKey("dbo.SafeCategories", t => t.SafeCategoryId, cascadeDelete: true)
                .Index(t => t.SafeCategoryId);
            
            CreateTable(
                "dbo.SafeCategories",
                c => new
                    {
                        SafeCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SafeCategoryId);
            
            AddColumn("dbo.Safes", "SafeSubCategoryId", c => c.Int());
            CreateIndex("dbo.Safes", "SafeSubCategoryId");
            AddForeignKey("dbo.Safes", "SafeSubCategoryId", "dbo.SafeSubCategories", "SafeSubCategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Safes", "SafeSubCategoryId", "dbo.SafeSubCategories");
            DropForeignKey("dbo.SafeSubCategories", "SafeCategoryId", "dbo.SafeCategories");
            DropIndex("dbo.SafeSubCategories", new[] { "SafeCategoryId" });
            DropIndex("dbo.Safes", new[] { "SafeSubCategoryId" });
            DropColumn("dbo.Safes", "SafeSubCategoryId");
            DropTable("dbo.SafeCategories");
            DropTable("dbo.SafeSubCategories");
        }
    }
}
