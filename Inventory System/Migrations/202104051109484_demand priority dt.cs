namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class demandprioritydt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DemandItems", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DemandItems", "DemandItemPriority", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DemandItems", "DemandItemPriority", c => c.Int(nullable: false));
            DropColumn("dbo.DemandItems", "DateCreated");
        }
    }
}
