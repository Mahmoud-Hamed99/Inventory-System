namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_doc_number_in_returns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemReturns", "DocumentNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemReturns", "DocumentNumber");
        }
    }
}
