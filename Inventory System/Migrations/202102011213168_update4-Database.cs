namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update4Database : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectFinished", c => c.Boolean(nullable: false));
            DropColumn("dbo.Projects", "ProjectOpen");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "ProjectOpen", c => c.Boolean(nullable: false));
            DropColumn("dbo.Projects", "ProjectFinished");
        }
    }
}
