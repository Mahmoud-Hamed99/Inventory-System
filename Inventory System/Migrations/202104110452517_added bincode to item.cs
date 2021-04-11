namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedbincodetoitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "BinCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "BinCode");
        }
    }
}
