namespace QAProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContentToAnswer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "Content", c => c.String(nullable: false, maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "Content");
        }
    }
}
