namespace DoAnNhom1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddata : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderProes", "PhoneCus", c => c.Int());
            RenameIndex(table: "dbo.OrderProes", name: "IX_IDCus", newName: "IX_Customer_IDCus");
            RenameColumn(table: "dbo.OrderProes", name: "IDCus", newName: "Customer_IDCus");
        }
    }
}
