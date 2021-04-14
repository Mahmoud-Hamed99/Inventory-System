namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedexchangetime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemOutputs", "ExchangeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemOutputs", "ExchangeDate");
        }
    }
}
