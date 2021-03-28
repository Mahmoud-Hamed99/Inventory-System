namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedprojectcodetoprojects : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ProjectCode");
        }
    }
}
