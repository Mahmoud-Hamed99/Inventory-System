namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addevalidationforitemtble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Items", "ItemName", c => c.String(nullable: false));
            AlterColumn("dbo.Items", "ItemUnit", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "ItemUnit", c => c.String());
            AlterColumn("dbo.Items", "ItemName", c => c.String());
        }
    }
}
