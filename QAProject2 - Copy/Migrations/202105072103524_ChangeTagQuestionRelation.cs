namespace QAProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTagQuestionRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionTags", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.QuestionTags", "TagId", "dbo.Tags");
            DropIndex("dbo.QuestionTags", new[] { "QuestionId" });
            DropIndex("dbo.QuestionTags", new[] { "TagId" });
            CreateTable(
                "dbo.TagQuestions",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Question_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Question_Id);
            
            DropTable("dbo.QuestionTags");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.QuestionTags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.TagQuestions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.TagQuestions", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagQuestions", new[] { "Question_Id" });
            DropIndex("dbo.TagQuestions", new[] { "Tag_Id" });
            DropTable("dbo.TagQuestions");
            CreateIndex("dbo.QuestionTags", "TagId");
            CreateIndex("dbo.QuestionTags", "QuestionId");
            AddForeignKey("dbo.QuestionTags", "TagId", "dbo.Tags", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionTags", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
        }
    }
}
