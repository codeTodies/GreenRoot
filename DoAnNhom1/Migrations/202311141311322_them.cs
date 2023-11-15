namespace DoAnNhom1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class them : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AdminUsers", "RoleUser", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdminUsers", "RoleUser", c => c.String(nullable: false));
            DropColumn("dbo.AdminUsers", "ConfirmPassword");
        }
    }
}
