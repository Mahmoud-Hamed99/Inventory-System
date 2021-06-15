namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeddoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemOutputs", "DocCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemOutputs", "DocCode");
        }
    }
}
