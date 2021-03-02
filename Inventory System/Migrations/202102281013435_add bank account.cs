namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addbankaccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        BankAccountId = c.Int(nullable: false, identity: true),
                        BankName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        TransitionNumber = c.Int(nullable: false),
                        Statement = c.String(),
                        TransitionType = c.String(),
                        Deposit = c.Double(nullable: false),
                        Withdraw = c.Double(nullable: false),
                        Balance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.BankAccountId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BankAccounts");
        }
    }
}
