namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedpurchase_approval_date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DemandItems", "PurchaseApprovalDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DemandItems", "PurchaseApprovalDate");
        }
    }
}
