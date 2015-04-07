namespace QHelper.UnitTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Table_Videos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Point = c.Int(nullable: false),
                        WatchedTimes = c.Long(nullable: false),
                        length = c.Single(nullable: false),
                        lendTimes = c.Int(),
                        SaledTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Videos");
        }
    }
}
