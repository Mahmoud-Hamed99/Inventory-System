namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDemandPlanningTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DemandPlannings",
                c => new
                    {
                        DemandPlanningId = c.Int(nullable: false, identity: true),
                        DemandItem = c.String(),
                        DemandItemQuantity = c.Double(nullable: false),
                        DemandItemPriority = c.Int(nullable: false),
                        DemandItemApprove = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DemandPlanningId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DemandPlannings");
        }
    }
}
