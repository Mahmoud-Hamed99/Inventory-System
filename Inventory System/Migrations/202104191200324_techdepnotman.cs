namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class techdepnotman : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ItemOutputs", "TechnicalDepartmentId", "dbo.TechnicalDepartments");
            DropIndex("dbo.ItemOutputs", new[] { "TechnicalDepartmentId" });
            AlterColumn("dbo.ItemOutputs", "TechnicalDepartmentId", c => c.Int());
            CreateIndex("dbo.ItemOutputs", "TechnicalDepartmentId");
            AddForeignKey("dbo.ItemOutputs", "TechnicalDepartmentId", "dbo.TechnicalDepartments", "TechnicalDepartmentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemOutputs", "TechnicalDepartmentId", "dbo.TechnicalDepartments");
            DropIndex("dbo.ItemOutputs", new[] { "TechnicalDepartmentId" });
            AlterColumn("dbo.ItemOutputs", "TechnicalDepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.ItemOutputs", "TechnicalDepartmentId");
            AddForeignKey("dbo.ItemOutputs", "TechnicalDepartmentId", "dbo.TechnicalDepartments", "TechnicalDepartmentId", cascadeDelete: true);
        }
    }
}
