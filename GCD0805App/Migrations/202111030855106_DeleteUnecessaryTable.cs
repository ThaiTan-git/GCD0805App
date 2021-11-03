namespace GCD0805App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteUnecessaryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainingCourses",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CourseId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainingCourses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TrainingCourses", "CourseId", "dbo.Courses");
            DropIndex("dbo.TrainingCourses", new[] { "CourseId" });
            DropIndex("dbo.TrainingCourses", new[] { "UserId" });
            DropTable("dbo.TrainingCourses");
        }
    }
}
