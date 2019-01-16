namespace ProyectoTPV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tpvmigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TicketVentas", "FechaHora", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketVentas", "FechaHora", c => c.DateTime(nullable: false));
        }
    }
}
