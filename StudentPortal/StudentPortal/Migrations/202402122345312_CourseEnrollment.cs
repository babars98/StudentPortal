namespace StudentPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseEnrollment : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CourseEnrollments", "CourseId");
            AddForeignKey("dbo.CourseEnrollments", "CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseEnrollments", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseEnrollments", new[] { "CourseId" });
        }
    }
}
