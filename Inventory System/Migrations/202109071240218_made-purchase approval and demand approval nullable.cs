namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class madepurchaseapprovalanddemandapprovalnullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DemandItems", "DemandItemApproval", c => c.Boolean());
            AlterColumn("dbo.DemandItems", "PurchasingApproval", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DemandItems", "PurchasingApproval", c => c.Boolean(nullable: false));
            AlterColumn("dbo.DemandItems", "DemandItemApproval", c => c.Boolean(nullable: false));
        }
    }
}
