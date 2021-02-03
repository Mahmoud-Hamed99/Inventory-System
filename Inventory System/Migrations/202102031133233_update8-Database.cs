namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update8Database : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemInputs", "ItemReturn", c => c.Double(nullable: false));
            AddColumn("dbo.ItemInputs", "Notes", c => c.String());
            AddColumn("dbo.ItemOutputs", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemOutputs", "Notes");
            DropColumn("dbo.ItemInputs", "Notes");
            DropColumn("dbo.ItemInputs", "ItemReturn");
        }
    }
}
