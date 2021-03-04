namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addchaeckIsPaiedAtrr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccounts", "CheckIsPaied", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankAccounts", "CheckIsPaied");
        }
    }
}
