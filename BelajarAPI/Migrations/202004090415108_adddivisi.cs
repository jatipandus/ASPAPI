namespace BelajarAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class adddivisi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tb_M_Divisi",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    DepartmentId = c.Int(nullable: false),
                    IsDelete = c.Boolean(nullable: false),
                    CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                    UpdateDate = c.DateTimeOffset(precision: 7),
                    DeleteDate = c.DateTimeOffset(precision: 7),

                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tb_M_Department", t => t.DepartmentId)
                .Index(t => t.DepartmentId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Tb_M_Divisi", "department_Id", "dbo.Tb_M_Department");
            DropIndex("dbo.Tb_M_Divisi", new[] { "department_Id" });
            DropTable("dbo.Tb_M_Divisi");
        }
    }
}
