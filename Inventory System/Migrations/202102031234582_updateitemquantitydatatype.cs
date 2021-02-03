namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateitemquantitydatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ItemInputs", "ItemQuantity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ItemInputs", "ItemQuantity", c => c.Int(nullable: false));
        }
    }
}
