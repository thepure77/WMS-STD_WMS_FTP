namespace FTPWebApplicationDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v0001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FTPFilesPaths",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileName = c.String(),
                        RelativePath = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FTPFilesPaths");
        }
    }
}
