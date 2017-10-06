namespace ProyectoTPV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Usuarios", "Nombre", c => c.String(nullable: false, maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuarios", "Nombre", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
