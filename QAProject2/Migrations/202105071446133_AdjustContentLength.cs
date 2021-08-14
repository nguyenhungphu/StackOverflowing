namespace QAProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustContentLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "Content", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "Content", c => c.String());
        }
    }
}
