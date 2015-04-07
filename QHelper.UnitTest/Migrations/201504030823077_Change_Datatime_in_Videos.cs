namespace QHelper.UnitTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Datatime_in_Videos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Videos", "SaledTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Videos", "SaledTime", c => c.DateTime(nullable: false));
        }
    }
}
