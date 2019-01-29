using System.Data.Entity;
using MySql.Data.EntityFramework;

namespace ProyectoTPV.Model
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class TpvEntities : DbContext
    {

        public TpvEntities() : base("rabbitPOS")
        {
            Database.SetInitializer<DbContext>(new CreateDatabaseIfNotExists<DbContext>());
        }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<LineaVenta> LineaVenta { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<TicketVenta> TicketVenta { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Caja> Caja { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<LineaPedidoProveedor> LineaPedidoProveedor { get; set; }
        public DbSet<Mesa> Mesa { get; set; }
        public DbSet<PedidoProveedor> PedidoProveedor { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Reserva> Reserver { get; set; }
        public DbSet<SubCategoria> SubCategoria { get; set; }
        public DbSet<VarianteProducto> VarianteProducto { get; set; }

   

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //one-to-many 
            modelBuilder.Entity<SubCategoria>()
                        .HasRequired<Categoria>(s => s.Categoria) // SubCategoria entity requires Categoria 
                        .WithMany(s => s.SubCategoria); // Categoria entity includes many SubCategoria entities

            modelBuilder.Entity<VarianteProducto>()
                      .HasRequired<Producto>(s => s.Producto)
                      .WithMany(s => s.VarianteProducto);

            base.OnModelCreating(modelBuilder);
        }
    }
}