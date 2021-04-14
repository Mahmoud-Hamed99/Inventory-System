namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_Log_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Action = c.String(),
                        UserId = c.Int(nullable: false),
                        TableName = c.String(),
                        RowId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserLogs");
        }
    }
}
