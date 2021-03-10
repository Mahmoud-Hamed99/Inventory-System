namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SafeModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Safes",
                c => new
                    {
                        SafeId = c.Int(nullable: false, identity: true),
                        PermessionNumber = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        TransactionType = c.String(),
                        Deposit = c.Double(nullable: false),
                        Withdraw = c.Double(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.SafeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Safes");
        }
    }
}
