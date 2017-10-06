namespace ProyectoTPV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        CategoriaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(),
                        RutaImagen = c.String(),
                    })
                .PrimaryKey(t => t.CategoriaId)
                .Index(t => t.Nombre, unique: true);
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        ProductoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Stock = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Iva = c.Int(nullable: false),
                        Descripcion = c.String(),
                        RutaImagen = c.String(),
                        Categoria_CategoriaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductoId)
                .ForeignKey("dbo.Categorias", t => t.Categoria_CategoriaId, cascadeDelete: true)
                .Index(t => t.Nombre, unique: true)
                .Index(t => t.Categoria_CategoriaId);
            
            CreateTable(
                "dbo.LineaVentas",
                c => new
                    {
                        LineaVentaId = c.Int(nullable: false, identity: true),
                        Unidades = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        TicketVentaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LineaVentaId)
                .ForeignKey("dbo.Productoes", t => t.ProductoID, cascadeDelete: true)
                .ForeignKey("dbo.TicketVentas", t => t.TicketVentaId, cascadeDelete: true)
                .Index(t => t.ProductoID)
                .Index(t => t.TicketVentaId);
            
            CreateTable(
                "dbo.TicketVentas",
                c => new
                    {
                        TicketVentaId = c.Int(nullable: false, identity: true),
                        FechaHora = c.DateTime(nullable: false),
                        Usuario_UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.TicketVentaId)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_UsuarioId)
                .Index(t => t.Usuario_UsuarioId);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        UsuarioId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        Apellidos = c.String(maxLength: 40),
                        Login = c.String(nullable: false, maxLength: 15),
                        Pin = c.String(nullable: false, maxLength: 4),
                        TipoUsuario = c.String(nullable: false),
                        Email = c.String(),
                        RutaImagen = c.String(),
                    })
                .PrimaryKey(t => t.UsuarioId)
                .Index(t => t.Login, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketVentas", "Usuario_UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.LineaVentas", "TicketVentaId", "dbo.TicketVentas");
            DropForeignKey("dbo.LineaVentas", "ProductoID", "dbo.Productoes");
            DropForeignKey("dbo.Productoes", "Categoria_CategoriaId", "dbo.Categorias");
            DropIndex("dbo.Usuarios", new[] { "Login" });
            DropIndex("dbo.TicketVentas", new[] { "Usuario_UsuarioId" });
            DropIndex("dbo.LineaVentas", new[] { "TicketVentaId" });
            DropIndex("dbo.LineaVentas", new[] { "ProductoID" });
            DropIndex("dbo.Productoes", new[] { "Categoria_CategoriaId" });
            DropIndex("dbo.Productoes", new[] { "Nombre" });
            DropIndex("dbo.Categorias", new[] { "Nombre" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.TicketVentas");
            DropTable("dbo.LineaVentas");
            DropTable("dbo.Productoes");
            DropTable("dbo.Categorias");
        }
    }
}
