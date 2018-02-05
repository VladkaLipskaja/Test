namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.URLs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        URLPath = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastTitle = c.String(),
                        LastStatus = c.Int(nullable: false),
                        LastUpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.URLUpdates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        URLId = c.Int(nullable: false),
                        Title = c.String(),
                        Status = c.Int(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.URLs", t => t.URLId, cascadeDelete: true)
                .Index(t => t.URLId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.URLUpdates", "URLId", "dbo.URLs");
            DropIndex("dbo.URLUpdates", new[] { "URLId" });
            DropTable("dbo.URLUpdates");
            DropTable("dbo.URLs");
        }
    }
}
