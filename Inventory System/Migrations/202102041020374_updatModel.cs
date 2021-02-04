namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ItemCategories", "ItemCategoryName", c => c.String(nullable: false));
            AlterColumn("dbo.ItemSubCategories", "ItemSubCategoryName", c => c.String(nullable: false));
            AlterColumn("dbo.Vendors", "VendorName", c => c.String(nullable: false));
            AlterColumn("dbo.Vendors", "VendorPhone", c => c.String(nullable: false));
            AlterColumn("dbo.Projects", "ProjectName", c => c.String(nullable: false));
            AlterColumn("dbo.TechnicalDepartments", "TechnicalDepartmentName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TechnicalDepartments", "TechnicalDepartmentName", c => c.String());
            AlterColumn("dbo.Projects", "ProjectName", c => c.String());
            AlterColumn("dbo.Vendors", "VendorPhone", c => c.String());
            AlterColumn("dbo.Vendors", "VendorName", c => c.String());
            AlterColumn("dbo.ItemSubCategories", "ItemSubCategoryName", c => c.String());
            AlterColumn("dbo.ItemCategories", "ItemCategoryName", c => c.String());
        }
    }
}
