namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update6Database : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "ItemReturn", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "ItemReturn");
        }
    }
}
