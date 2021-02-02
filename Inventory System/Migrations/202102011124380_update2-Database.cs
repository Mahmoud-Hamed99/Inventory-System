namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2Database : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectOpen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ProjectOpen");
        }
    }
}
