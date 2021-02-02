namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update3Database : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TechnicalDepartments", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TechnicalDepartments", "DateCreated");
        }
    }
}
