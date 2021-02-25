namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpurchasingattribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DemandItems", "PurchasingApproval", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DemandItems", "PurchasingApproval");
        }
    }
}
