namespace DoAnNhom1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _finally : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuantityTrees",
                c => new
                    {
                        TreeID = c.Int(nullable: false, identity: true),
                        NameTree = c.String(),
                        Price = c.Int(nullable: false),
                        ImageTree = c.String(),
                        NameArea = c.String(),
                        Total_money = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sum_Quantity = c.Int(),
                        area_IDArea = c.Int(),
                        order_ID = c.Int(),
                        tree_TreeID = c.Int(),
                    })
                .PrimaryKey(t => t.TreeID)
                .ForeignKey("dbo.Areas", t => t.area_IDArea)
                .ForeignKey("dbo.OrderDetails", t => t.order_ID)
                .ForeignKey("dbo.Trees", t => t.tree_TreeID)
                .Index(t => t.area_IDArea)
                .Index(t => t.order_ID)
                .Index(t => t.tree_TreeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuantityTrees", "tree_TreeID", "dbo.Trees");
            DropForeignKey("dbo.QuantityTrees", "order_ID", "dbo.OrderDetails");
            DropForeignKey("dbo.QuantityTrees", "area_IDArea", "dbo.Areas");
            DropIndex("dbo.QuantityTrees", new[] { "tree_TreeID" });
            DropIndex("dbo.QuantityTrees", new[] { "order_ID" });
            DropIndex("dbo.QuantityTrees", new[] { "area_IDArea" });
            DropTable("dbo.QuantityTrees");
        }
    }
}
