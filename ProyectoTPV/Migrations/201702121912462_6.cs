namespace ProyectoTPV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Usuarios", "Pin", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuarios", "Pin", c => c.String(nullable: false, maxLength: 4));
        }
    }
}
