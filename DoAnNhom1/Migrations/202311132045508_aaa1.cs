namespace DoAnNhom1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaa1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdminUsers", "ConfirmPassword", c => c.String(nullable: false));
            AlterColumn("dbo.AdminUsers", "RoleUser", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdminUsers", "RoleUser", c => c.String(nullable: false));
            DropColumn("dbo.AdminUsers", "ConfirmPassword");
        }
    }
}
