namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDocCodetoItemInputTale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemInputs", "DocCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemInputs", "DocCode");
        }
    }
}
