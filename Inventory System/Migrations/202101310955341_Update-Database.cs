namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TechnicalDepartments",
                c => new
                    {
                        TechnicalDepartmentId = c.Int(nullable: false, identity: true),
                        TechnicalDepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.TechnicalDepartmentId);
            
            AddColumn("dbo.ItemOutputs", "TechnicalDepartmentId", c => c.Int(nullable: false));
            AddColumn("dbo.ItemOutputs", "ItemOutputApproved", c => c.Boolean(nullable: false));
            CreateIndex("dbo.ItemOutputs", "TechnicalDepartmentId");
            AddForeignKey("dbo.ItemOutputs", "TechnicalDepartmentId", "dbo.TechnicalDepartments", "TechnicalDepartmentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemOutputs", "TechnicalDepartmentId", "dbo.TechnicalDepartments");
            DropIndex("dbo.ItemOutputs", new[] { "TechnicalDepartmentId" });
            DropColumn("dbo.ItemOutputs", "ItemOutputApproved");
            DropColumn("dbo.ItemOutputs", "TechnicalDepartmentId");
            DropTable("dbo.TechnicalDepartments");
        }
    }
}
