namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateItemReturns : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ItemReturns", "projectId", "dbo.Projects");
            DropIndex("dbo.ItemReturns", new[] { "projectId" });
            AddColumn("dbo.ItemReturns", "ItemInputId", c => c.Int());
            AlterColumn("dbo.ItemReturns", "projectId", c => c.Int());
            CreateIndex("dbo.ItemReturns", "projectId");
            CreateIndex("dbo.ItemReturns", "ItemInputId");
            AddForeignKey("dbo.ItemReturns", "ItemInputId", "dbo.ItemInputs", "ItemInputId");
            AddForeignKey("dbo.ItemReturns", "projectId", "dbo.Projects", "ProjectId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemReturns", "projectId", "dbo.Projects");
            DropForeignKey("dbo.ItemReturns", "ItemInputId", "dbo.ItemInputs");
            DropIndex("dbo.ItemReturns", new[] { "ItemInputId" });
            DropIndex("dbo.ItemReturns", new[] { "projectId" });
            AlterColumn("dbo.ItemReturns", "projectId", c => c.Int(nullable: false));
            DropColumn("dbo.ItemReturns", "ItemInputId");
            CreateIndex("dbo.ItemReturns", "projectId");
            AddForeignKey("dbo.ItemReturns", "projectId", "dbo.Projects", "ProjectId", cascadeDelete: true);
        }
    }
}
