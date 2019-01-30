namespace ProyectoTPV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cajas",
                c => new
                    {
                        CajaId = c.Int(nullable: false, identity: true),
                        DineroInicial = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DineroFinal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaHoraApertura = c.DateTime(nullable: false, precision: 0),
                        FechaHoraRecuento = c.DateTime(precision: 0),
                        Vendedor_UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.CajaId)
                .ForeignKey("dbo.Usuarios", t => t.Vendedor_UsuarioId)
                .Index(t => t.Vendedor_UsuarioId);
            
            CreateTable(
                "dbo.TicketsVentas",
                c => new
                    {
                        TicketVentaId = c.Int(nullable: false, identity: true),
                        FechaHora = c.DateTime(nullable: false, precision: 0),
                        Cliente_ClienteId = c.Int(),
                        Mesa_MesaId = c.Int(),
                        Usuario_UsuarioId = c.Int(),
                        Caja_CajaId = c.Int(),
                    })
                .PrimaryKey(t => t.TicketVentaId)
                .ForeignKey("dbo.Clientes", t => t.Cliente_ClienteId)
                .ForeignKey("dbo.Mesas", t => t.Mesa_MesaId)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_UsuarioId)
                .ForeignKey("dbo.Cajas", t => t.Caja_CajaId)
                .Index(t => t.Cliente_ClienteId)
                .Index(t => t.Mesa_MesaId)
                .Index(t => t.Usuario_UsuarioId)
                .Index(t => t.Caja_CajaId);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        ClienteId = c.Int(nullable: false, identity: true),
                        Puntos = c.Int(nullable: false),
                        Nombre = c.String(maxLength: 255, storeType: "nvarchar"),
                        Apellidos = c.String(maxLength: 255, storeType: "nvarchar"),
                        Direccion = c.String(maxLength: 255, storeType: "nvarchar"),
                        Dni = c.String(maxLength: 255, storeType: "nvarchar"),
                        Telefono = c.String(maxLength: 255, storeType: "nvarchar"),
                        Email = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ClienteId);
            
            CreateTable(
                "dbo.LineaVentas",
                c => new
                    {
                        LineaVentaId = c.Int(nullable: false, identity: true),
                        Unidades = c.Int(nullable: false),
                        VarianteProductoId = c.Int(nullable: false),
                        TicketVentaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LineaVentaId)
                .ForeignKey("dbo.TicketsVentas", t => t.TicketVentaId, cascadeDelete: true)
                .ForeignKey("dbo.VarianteProductoes", t => t.VarianteProductoId, cascadeDelete: true)
                .Index(t => t.VarianteProductoId)
                .Index(t => t.TicketVentaId);
            
            CreateTable(
                "dbo.VarianteProductoes",
                c => new
                    {
                        VarianteProductoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(unicode: false),
                        Descripcion = c.String(unicode: false),
                        RutaImagen = c.String(unicode: false),
                        Stock = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Producto_ProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VarianteProductoId)
                .ForeignKey("dbo.Productos", t => t.Producto_ProductoId, cascadeDelete: true)
                .Index(t => t.Producto_ProductoId);
            
            CreateTable(
                "dbo.Productos",
                c => new
                    {
                        ProductoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Stock = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Iva = c.Int(nullable: false),
                        Descripcion = c.String(unicode: false),
                        RutaImagen = c.String(unicode: false),
                        Proveedor_ProveedorId = c.Int(),
                        SubCategoria_SubCategoriaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductoId)
                .ForeignKey("dbo.Proveedores", t => t.Proveedor_ProveedorId)
                .ForeignKey("dbo.SubCategorias", t => t.SubCategoria_SubCategoriaId, cascadeDelete: true)
                .Index(t => t.Nombre, unique: true)
                .Index(t => t.Proveedor_ProveedorId)
                .Index(t => t.SubCategoria_SubCategoriaId);
            
            CreateTable(
                "dbo.Proveedores",
                c => new
                    {
                        ProveedorId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(unicode: false),
                        NombreContacto = c.String(unicode: false),
                        Cif = c.String(unicode: false),
                        Direccion = c.String(unicode: false),
                        Telefono = c.String(unicode: false),
                        Email = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ProveedorId);
            
            CreateTable(
                "dbo.SubCategorias",
                c => new
                    {
                        SubCategoriaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(unicode: false),
                        Descripcion = c.String(unicode: false),
                        RutaImagen = c.String(unicode: false),
                        Categoria_CategoriaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubCategoriaId)
                .ForeignKey("dbo.Categorias", t => t.Categoria_CategoriaId, cascadeDelete: true)
                .Index(t => t.Categoria_CategoriaId);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        CategoriaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Descripcion = c.String(maxLength: 255, storeType: "nvarchar"),
                        RutaImagen = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.CategoriaId)
                .Index(t => t.Nombre, unique: true);
            
            CreateTable(
                "dbo.Mesas",
                c => new
                    {
                        MesaId = c.Int(nullable: false, identity: true),
                        NombreMesa = c.String(unicode: false),
                        Capacidad = c.Int(nullable: false),
                        IncrementoMesa = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.MesaId);
            
            CreateTable(
                "dbo.Reservas",
                c => new
                    {
                        ReservaId = c.Int(nullable: false, identity: true),
                        NombreReserva = c.String(unicode: false),
                        FechaHora = c.DateTime(nullable: false, precision: 0),
                        Comentarios = c.String(unicode: false),
                        Mesa_MesaId = c.Int(),
                    })
                .PrimaryKey(t => t.ReservaId)
                .ForeignKey("dbo.Mesas", t => t.Mesa_MesaId)
                .Index(t => t.Mesa_MesaId);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        UsuarioId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        Apellidos = c.String(maxLength: 40, storeType: "nvarchar"),
                        Login = c.String(nullable: false, maxLength: 15, storeType: "nvarchar"),
                        Pin = c.String(nullable: false, unicode: false),
                        TipoUsuario = c.String(nullable: false, unicode: false),
                        Email = c.String(unicode: false),
                        RutaImagen = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.UsuarioId)
                .Index(t => t.Login, unique: true);
            
            CreateTable(
                "dbo.LineaPedidoProveedores",
                c => new
                    {
                        LineaPedidoProveedorId = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(nullable: false),
                        Comentario = c.String(maxLength: 255, storeType: "nvarchar"),
                        PedidoProveedor_PedidoProveedorId = c.Int(),
                        VarianteProducto_VarianteProductoId = c.Int(),
                    })
                .PrimaryKey(t => t.LineaPedidoProveedorId)
                .ForeignKey("dbo.PedidoProveedores", t => t.PedidoProveedor_PedidoProveedorId)
                .ForeignKey("dbo.VarianteProductoes", t => t.VarianteProducto_VarianteProductoId)
                .Index(t => t.PedidoProveedor_PedidoProveedorId)
                .Index(t => t.VarianteProducto_VarianteProductoId);
            
            CreateTable(
                "dbo.PedidoProveedores",
                c => new
                    {
                        PedidoProveedorId = c.Int(nullable: false, identity: true),
                        FechaPedido = c.DateTime(nullable: false, precision: 0),
                        Recibido = c.Boolean(nullable: false),
                        Proveedor_ProveedorId = c.Int(),
                    })
                .PrimaryKey(t => t.PedidoProveedorId)
                .ForeignKey("dbo.Proveedores", t => t.Proveedor_ProveedorId)
                .Index(t => t.Proveedor_ProveedorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LineaPedidoProveedores", "VarianteProducto_VarianteProductoId", "dbo.VarianteProductoes");
            DropForeignKey("dbo.PedidoProveedores", "Proveedor_ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.LineaPedidoProveedores", "PedidoProveedor_PedidoProveedorId", "dbo.PedidoProveedores");
            DropForeignKey("dbo.Cajas", "Vendedor_UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.TicketsVentas", "Caja_CajaId", "dbo.Cajas");
            DropForeignKey("dbo.TicketsVentas", "Usuario_UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.TicketsVentas", "Mesa_MesaId", "dbo.Mesas");
            DropForeignKey("dbo.Reservas", "Mesa_MesaId", "dbo.Mesas");
            DropForeignKey("dbo.LineaVentas", "VarianteProductoId", "dbo.VarianteProductoes");
            DropForeignKey("dbo.VarianteProductoes", "Producto_ProductoId", "dbo.Productos");
            DropForeignKey("dbo.Productos", "SubCategoria_SubCategoriaId", "dbo.SubCategorias");
            DropForeignKey("dbo.SubCategorias", "Categoria_CategoriaId", "dbo.Categorias");
            DropForeignKey("dbo.Productos", "Proveedor_ProveedorId", "dbo.Proveedores");
            DropForeignKey("dbo.LineaVentas", "TicketVentaId", "dbo.TicketsVentas");
            DropForeignKey("dbo.TicketsVentas", "Cliente_ClienteId", "dbo.Clientes");
            DropIndex("dbo.PedidoProveedores", new[] { "Proveedor_ProveedorId" });
            DropIndex("dbo.LineaPedidoProveedores", new[] { "VarianteProducto_VarianteProductoId" });
            DropIndex("dbo.LineaPedidoProveedores", new[] { "PedidoProveedor_PedidoProveedorId" });
            DropIndex("dbo.Usuarios", new[] { "Login" });
            DropIndex("dbo.Reservas", new[] { "Mesa_MesaId" });
            DropIndex("dbo.Categorias", new[] { "Nombre" });
            DropIndex("dbo.SubCategorias", new[] { "Categoria_CategoriaId" });
            DropIndex("dbo.Productos", new[] { "SubCategoria_SubCategoriaId" });
            DropIndex("dbo.Productos", new[] { "Proveedor_ProveedorId" });
            DropIndex("dbo.Productos", new[] { "Nombre" });
            DropIndex("dbo.VarianteProductoes", new[] { "Producto_ProductoId" });
            DropIndex("dbo.LineaVentas", new[] { "TicketVentaId" });
            DropIndex("dbo.LineaVentas", new[] { "VarianteProductoId" });
            DropIndex("dbo.TicketsVentas", new[] { "Caja_CajaId" });
            DropIndex("dbo.TicketsVentas", new[] { "Usuario_UsuarioId" });
            DropIndex("dbo.TicketsVentas", new[] { "Mesa_MesaId" });
            DropIndex("dbo.TicketsVentas", new[] { "Cliente_ClienteId" });
            DropIndex("dbo.Cajas", new[] { "Vendedor_UsuarioId" });
            DropTable("dbo.PedidoProveedores");
            DropTable("dbo.LineaPedidoProveedores");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Reservas");
            DropTable("dbo.Mesas");
            DropTable("dbo.Categorias");
            DropTable("dbo.SubCategorias");
            DropTable("dbo.Proveedores");
            DropTable("dbo.Productos");
            DropTable("dbo.VarianteProductoes");
            DropTable("dbo.LineaVentas");
            DropTable("dbo.Clientes");
            DropTable("dbo.TicketsVentas");
            DropTable("dbo.Cajas");
        }
    }
}
